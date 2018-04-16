using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TransactionsManager.Domain;
using TransactionsManager.Services.Interfaces;
using X.PagedList;

namespace TransactionsManager.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionRepository _repository;

        public TransactionsService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IPagedList<Transaction>> Find(TransactionsSearchCriteria searchCriteria, int pageNumber, int pageSize)
        {
            return await _repository.Find(GetPredicate(searchCriteria), pageNumber, pageSize);
        }

        private Expression<Func<Transaction, bool>> GetPredicate(TransactionsSearchCriteria searchCriteria)
        {
            if (searchCriteria.IsFraud.HasValue && !string.IsNullOrEmpty(searchCriteria.SearchNameDest))
            {
                return (t) => t.IsFraud == searchCriteria.IsFraud
                    && t.NameDest == searchCriteria.SearchNameDest;
            }
            if (searchCriteria.IsFraud.HasValue && string.IsNullOrEmpty(searchCriteria.SearchNameDest))
                return (t) => t.IsFraud == searchCriteria.IsFraud;
            else
                return (t) => t.NameDest == searchCriteria.SearchNameDest;
        }

        public async Task MarkAsFraud(int id)
        {
            var transaction = await _repository.FindById(id);
            if (transaction == null) return;
            transaction.IsFraud = true;
            await _repository.Update(transaction);
        }

        public async Task<IPagedList<Transaction>> Paginate(int pageNumber, int pageSize)
        {
            return await _repository.Paginate(pageNumber, pageSize);
        }

        public async Task Register(Transaction transaction)
        {
            await _repository.Register(transaction);
        }

        public async Task<Transaction> FindById(int id)
        {
            return await _repository.FindById(id);
        }
    }
}
