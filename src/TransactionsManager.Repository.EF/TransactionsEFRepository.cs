using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TransactionsManager.Domain;
using X.PagedList;

namespace TransactionsManager.Repository.EF
{
    public class TransactionsEFRepository : ITransactionRepository
    {
        private readonly TransactionManagerContext _productsContext;

        public TransactionsEFRepository(TransactionManagerContext productsContext)
        {
            _productsContext = productsContext;
        }

        public async Task<IPagedList<Transaction>> Find(Expression<Func<Transaction, bool>> predicate,
            int pageNumber,
            int pageSize)
        {
            return await Task.Run(() => _productsContext.Transactions
                .Where(predicate)
                .OrderByDescending(t => t.Id)
                .ToPagedList(pageNumber, pageSize));
        }

        public async Task<Transaction> FindById(int id)
        {
            return await _productsContext.Transactions.FindAsync(id);
        }

        public async Task<IPagedList<Transaction>> Paginate(int pageNumber, int pageSize)
        {
            return await Task.Run(() => _productsContext.Transactions.OrderByDescending(t => t.Id).ToPagedList(pageNumber, pageSize));
        }

        public async Task Register(Transaction transaction)
        {
            _productsContext.Add(transaction);
            await _productsContext.SaveChangesAsync();
        }

        public async Task Update(Transaction transaction)
        {
            var found = _productsContext.Transactions.FirstOrDefault(t => t.Id == transaction.Id);
            if (found == null) return;
            var entry = _productsContext.Entry(transaction);
            entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _productsContext.SaveChangesAsync();
        }
    }
}
