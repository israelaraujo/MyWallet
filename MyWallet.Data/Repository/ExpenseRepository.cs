using MyWallet.Data.Domain;
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
            var expenses = _session.Query<Expense>()
                .Include(e => e.ContextId)
                .Include(e => e.BankAccountId)
                .Include(e => e.CategoryId)
                .Where(e => e.ContextId == contextId)
                .ToList();

            foreach (var e in expenses)
            {
                e.Context = _session.Load<Context>(e.ContextId);
                e.BankAccount = _session.Load<BankAccount>(e.BankAccountId);
                e.Category = _session.Load<Category>(e.CategoryId);
            }

            return expenses;
        }

    }
}
