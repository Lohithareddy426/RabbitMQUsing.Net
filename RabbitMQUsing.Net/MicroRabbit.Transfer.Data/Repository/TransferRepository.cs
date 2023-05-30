using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private TransferDbContext _transferDbContext;
        public TransferRepository(TransferDbContext transferDbContext) {

            _transferDbContext = transferDbContext;

        }

        public IEnumerable<TransferLog> GetTransferLogs()
        {
            return _transferDbContext.TransferLogs;
        }

        public void Add(TransferLog log)
        {
             _transferDbContext.Add(log);
            _transferDbContext.SaveChanges();   

        }

       
    }
}
