using System;

namespace TransactionsManager.Domain
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Step { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string NameOrig { get; set; }
        public double OldbalanceOrg { get; set; }
        public double NewbalanceOrig { get; set; }
        public string NameDest { get; set; }
        public double OldbalanceDest { get; set; }
        public double NewbalanceDest { get; set; }
        public bool? IsFraud { get; set; }
        public bool? IsFlaggedFraud { get; set; }
    }
}
