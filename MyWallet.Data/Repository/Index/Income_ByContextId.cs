using MyWallet.Data.Domain;
using Raven.Client.Documents.Indexes;
using System;
using System.Linq;

namespace MyWallet.Data.Repository.Index
{
    public class Income_ByContextId : AbstractIndexCreationTask<Income, Income_ByContextId.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public string ContextId { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public DateTime Date { get; set; }
            public bool Received { get; set; }
            public string BankAccount { get; set; }
            public string Category { get; set; }
        }

        public Income_ByContextId()
        {
            Map = incomes => from i in incomes
                             select new
                             {
                                 i.Id,
                                 i.ContextId,
                                 i.Description,
                                 i.Value,
                                 i.Date,
                                 i.Received,
                                 BankAccount = LoadDocument<BankAccount>(i.BankAccountId).Name,
                                 Category = LoadDocument<Category>(i.CategoryId).Name
                             };

            Reduce = results => from r in results
                                group r by new
                                {
                                    r.Id,
                                    r.ContextId,
                                    r.Description,
                                    r.Value,
                                    r.Date,
                                    r.Received,
                                    r.BankAccount,
                                    r.Category,
                                }
                                into g
                                select new
                                {
                                    g.Key.Id,
                                    g.Key.ContextId,
                                    g.Key.Description,
                                    g.Key.Value,
                                    g.Key.Date,
                                    g.Key.Received,
                                    g.Key.BankAccount,
                                    g.Key.Category,
                                };
        }
    }
}
