using MyWallet.Data.Domain;
using Raven.Client.Documents.Indexes;
using System;
using System.Linq;

namespace MyWallet.Data.Repository.Index
{
    public class Expense_ByContextId : AbstractIndexCreationTask<Domain.Expense, Expense_ByContextId.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public string ContextId { get; set; }
            public string Description { get; set; }
            public string CategoryId { get; set; }
            public bool IsPaid { get; set; }
            public string BankAccountId { get; set; }
            public decimal Value { get; set; }
            public DateTime Date { get; set; }
            public string BankAccount { get; set; }
            public string Category { get; set; }
        }

        public Expense_ByContextId()
        {
            Map = expenses => from expense in expenses
                              select new
                              {
                                  expense.Id,
                                  expense.ContextId,
                                  expense.Description,
                                  expense.CategoryId,
                                  expense.IsPaid,
                                  expense.BankAccountId,
                                  expense.Value,
                                  expense.Date,
                                  BankAccount = LoadDocument<BankAccount>(expense.BankAccountId).Name,
                                  Category = LoadDocument<Category>(expense.CategoryId).Name
                              };

            Reduce = results => from result in results
                                group result by new
                                {
                                    result.Id,
                                    result.ContextId,
                                    result.Description,
                                    result.CategoryId,
                                    result.IsPaid,
                                    result.BankAccountId,
                                    result.Value,
                                    result.Date,
                                    result.BankAccount,
                                    result.Category
                                }
                                into g
                                select new
                                {
                                    g.Key.Id,
                                    g.Key.ContextId,
                                    g.Key.Description,
                                    g.Key.CategoryId,
                                    g.Key.IsPaid,
                                    g.Key.BankAccountId,
                                    g.Key.Value,
                                    g.Key.Date,
                                    g.Key.BankAccount,
                                    g.Key.Category
                                };
        }
    }
}
