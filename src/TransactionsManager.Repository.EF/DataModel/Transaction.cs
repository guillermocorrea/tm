using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransactionsManager.Repository.EF.DataModel
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
