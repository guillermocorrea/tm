using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace TransactionsManager.Domain
{
    public interface ITransactionRepository
    {
        Task Register(Transaction transaction);
        Task<IPagedList<Transaction>> Paginate(int pageNumber, int pageSize);
        Task<IPagedList<Transaction>> Find(Expression<Func<Transaction, bool>> predicate, int pageNumber, int pageSize);
        Task<Transaction> FindById(int id);
        Task Update(Transaction transaction);
    }
}
