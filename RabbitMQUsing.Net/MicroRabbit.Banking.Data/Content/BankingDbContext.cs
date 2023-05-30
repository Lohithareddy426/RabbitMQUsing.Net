using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Data.Content
{
    public class BankingDbContext : DbContext
    {       
        public BankingDbContext(DbContextOptions options ) : base( options ) 
        {
        
        } 
        public DbSet<Account> Accounts { get; set; }

        
    }
}
