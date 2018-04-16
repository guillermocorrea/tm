using Microsoft.EntityFrameworkCore;
using System;
using TransactionsManager.Domain;

namespace TransactionsManager.Repository.EF
{
    public class TransactionManagerContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public TransactionManagerContext(DbContextOptions<TransactionManagerContext> options) : base(options)
        {
        }
    }
}
