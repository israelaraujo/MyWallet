using MyWallet.Data.Domain;
using MyWallet.Data.Repository.Index;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class ExpenseRepository
    {
        private IDocumentSession _session;

        public ExpenseRepository(IDocumentSession session)
        {
            _session = session;
        }

        public void Save(Expense expense)
        {
            _session.Store(expense);
        }

        public void Delete(string id)
        {
            _session.Delete(id);
        }

        public Expense GetById(string id)
        {
            return _session.Load<Expense>(id);
        }

        public IEnumerable<Expense> GetAllByContextId(string contextId)
        {
            var result = _session
                .Query<Expense_ByContextId.Result, Expense_ByContextId>()
                .Where(x => x.ContextId == contextId)
                .Select(x => new Expense
                {
                    Id = x.Id,
                    Description = x.Description,
                    ContextId = x.ContextId,
                    BankAccount = new BankAccount { Name = x.BankAccount },
                    Category = new Category { Name = x.Category },
                    IsPaid = x.IsPaid,
                    Value = x.Value,
                    Date = x.Date
                })
                .ToList();

            return result;
        }

    }
}
