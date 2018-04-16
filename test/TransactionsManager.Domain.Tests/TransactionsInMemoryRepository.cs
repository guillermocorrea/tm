using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace TransactionsManager.Domain.Tests
{
    /// <summary>
    /// In memory mock implementation of the ITransactionRepository interface.
    /// </summary>
    public class TransactionsInMemoryRepository : ITransactionRepository
    {
        private IList<Transaction> _transactions;

        public TransactionsInMemoryRepository()
        {
            _transactions = new List<Transaction>();
            InitMockData();
        }

        private void InitMockData()
        {
            for (int i = 1; i <= 20; i++)
            {
                var mockTransaction = new Transaction()
                {
                    Id = i,
                    IsFraud = i % 2 == 0,
                    NameDest = $"Name {i}"
                };
                _transactions.Add(mockTransaction);
            }
        }

        public async Task<IPagedList<Transaction>> Find(Expression<Func<Transaction, bool>> predicate, int pageNumber, int pageSize)
        {
            return await Task.Run(() => _transactions
                .AsQueryable()
                .Where(predicate)
                .ToPagedList(pageNumber, pageSize));
        }

        public async Task Register(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public async Task Update(Transaction transaction)
        {
            var foundTransaction = _transactions.FirstOrDefault(t => t.Id == transaction.Id);
            if (foundTransaction == null) return;
            var idx = _transactions.IndexOf(foundTransaction);
            if (idx != -1) _transactions[idx] = transaction;
        }

        public async Task<IPagedList<Transaction>> Paginate(int pageNumber, int pageSize)
        {
            return await Task.Run(() => _transactions.ToPagedList(pageNumber, pageSize));
        }

        public async Task<Transaction> FindById(int id)
        {
            return await Task.Run(() => _transactions.First(t => t.Id == id));
        }
    }
}
