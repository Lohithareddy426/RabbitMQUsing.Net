using MicroRabbit.Banking.Data.Content;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private  BankingDbContext _bankingDbContext;
        public AccountRepository(BankingDbContext bankingDbContext) { 

            _bankingDbContext = bankingDbContext;

        }

        public IEnumerable<Account> GetAccounts()
        {
            return _bankingDbContext.Accounts;
        }

        public string SaveAccount(Account account)
        {
            try
            {
                _bankingDbContext.Add(account);
                _bankingDbContext.SaveChanges();
                return ("Account saved successfully");
            }
            catch (Exception ex)
            {
                return (ex.InnerException.Message);
            }
           
        }
    }
}
