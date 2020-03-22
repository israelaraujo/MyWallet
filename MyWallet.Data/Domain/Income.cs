using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Data.Domain
{
    public class Income
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public bool Received { get; set; }
        public DateTime CreationDate { get; set; }
        public string Observation { get; set; }
        public string ContextId { get; set; }
        public Context Context { get; set; }
        public string BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
