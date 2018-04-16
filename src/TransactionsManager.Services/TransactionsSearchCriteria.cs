using System;

namespace TransactionsManager.Services
{
    public class TransactionsSearchCriteria
    {
        public bool? IsFraud { get; set; }
        public string SearchNameDest { get; set; }
    }
}
