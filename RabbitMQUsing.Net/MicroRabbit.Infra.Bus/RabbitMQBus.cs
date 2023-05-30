using MediatR;
using MicroRabbitMQ.Domain.Core.Bus.Interface;
using MicroRabbitMQ.Domain.Core.Commands;
using MicroRabbitMQ.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQBus( IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        public Task SendCommand<T>(T Command) where T : Command
        {
            return _mediator.Send(Command);            
        }


        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { };
            using(var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
               var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("",eventName, null,body);
            }
        }      

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
           
            var eventName =typeof(T).Name;
            var handlerType = typeof(TH);

            if(!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T)); 
            }

            if(!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(x => x.GetType() == handlerType))
            {
                throw new Exception(
                    $"Handler Type {handlerType.Name} already is registered for {eventName}");
            }

            _handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
           
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName,false,false,false,null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += Consumer_Recieved;

            channel.BasicConsume(eventName, true, consumer);


        }

        private  async Task Consumer_Recieved(object sender, BasicDeliverEventArgs ae)
        {
            var eventName = ae.RoutingKey;
            var message = Encoding.UTF8.GetString(ae.Body.ToArray());

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);

            }
            catch (Exception ex)
            {

            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if(_handlers.ContainsKey(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var sunbscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(sunbscription);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);  
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var conreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[] {@event });
                    }
                }
            }
        }



    }
}