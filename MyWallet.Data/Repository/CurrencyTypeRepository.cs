using MyWallet.Data.Domain;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Documents;
using MyWallet.Data.Repository.Index;

namespace MyWallet.Data.Repository
{
    public class CurrencyTypeRepository
    {
        private IDocumentSession _session;

        public CurrencyTypeRepository(IDocumentSession session)
        {
            _session = session;
        }

        public IEnumerable<CurrencyType> GetAll()
        {
            return _session.Query<CurrencyType>().ToList();
        }

        public string GetCurrencySymbolByContextId(string contextId)
        {
            return _session
                .Query<Currency_GetSymbolByContextId.Result, Currency_GetSymbolByContextId>()
                .Where(x => x.ContextId == contextId)
                .Select(x => x.CurrencySymbol)
                .FirstOrDefault();
        }
    }
}
