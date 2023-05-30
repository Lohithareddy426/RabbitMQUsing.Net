using MicroRabbitMQ.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Domain.Events
{
    public class TransferCreatedEvent : Event
    {
        public int From { get; private set; }
        public decimal Amount { get; private set; }
        public int To { get; private set; }

        public TransferCreatedEvent(int from,int to ,decimal amount)
        {
            this.From = from;
            this.Amount = amount;
            this.To = to;
        }
    }
}
