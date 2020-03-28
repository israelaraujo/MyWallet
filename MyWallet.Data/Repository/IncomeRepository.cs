using MyWallet.Data.Domain;
using MyWallet.Data.Repository.Index;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class IncomeRepository
    {
        private IDocumentSession _session;

        public IncomeRepository(IDocumentSession session)
        {
            _session = session;
        }

        public Income GetById(string id)
        {
            return _session.Load<Income>(id);
        }

        public void Save(Income income)
        {
            _session.Store(income);
        }

        public void Delete(string incomeId)
        {
            _session.Delete(incomeId);
        }

        public IEnumerable<Income> GetByContextId(string contextId)
        {
            return _session.Query<Income_ByContextId.Result, Income_ByContextId>()
                .Where(x => x.ContextId == contextId)
                .Select(x => new Income
                {
                    Id = x.Id,
                    Description = x.Description,
                    Value = x.Value,
                    Date = x.Date,
                    Received = x.Received,
                    BankAccount = new BankAccount { Name = x.BankAccount },
                    Category = new Category { Name = x.Category },
                })
                .ToList();
        }
    }
}
