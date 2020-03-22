using MyWallet.Data.Domain;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
            return _session.Query<Expense>().Where(e => e.ContextId == contextId);
        }

    }
}
