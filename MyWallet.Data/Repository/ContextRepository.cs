using MyWallet.Data.Domain;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class ContextRepository
    {
        private IDocumentSession session;

        public ContextRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public void Save(Context context)
        {
            session.Store(context);
        }

        public void Delete(Context context)
        {
            session.Delete(context);
        }

        public Context GetById(string id)
        {
            return session.Load<Context>(id);
        }

        public IEnumerable<Context> GetByUserId(string userId)
        {
            //return session.Query<Context>()
            //    .Where(c => c.UserId == userId)
            //    .ToList();
            return null;
        }

        public void SetTheMainContextAsNonMain(string userId)
        {
            //var mainContext = _context.Context.FirstOrDefault(c => c.UserId == userId && c.IsMainContext);
            //mainContext.IsMainContext = false;
        }
    }
}
