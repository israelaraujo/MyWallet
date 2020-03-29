using MyWallet.Data.Domain;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class ContextRepository
    {
        private IDocumentSession _session;

        public ContextRepository(IDocumentSession session)
        {
            _session = session;
        }

        public void Save(Context context)
        {
            _session.Store(context);
        }

        public void Delete(Context context)
        {
            _session.Delete(context);
        }

        public Context GetById(string id)
        {
            return _session.Load<Context>(id);
        }

        public IEnumerable<Context> GetWithCurrencyByUserId(string userId)
        {
            var contexts = _session.Query<Context>()
                .Include(c => c.CurrencyTypeId)
                .Where(c => c.UserId == userId)
                .ToList();

            foreach (var context in contexts)
                context.CurrencyType = _session.Load<CurrencyType>(context.CurrencyTypeId);

            return contexts;
        }

        public void SetTheMainContextAsNonMain(string userId)
        {
            var contexts = _session.Query<Context>().Where(c => c.UserId == userId);

            foreach (var context in contexts)
            {
                context.IsMainContext = false;
                Save(context);
            }
        }
    }
}
