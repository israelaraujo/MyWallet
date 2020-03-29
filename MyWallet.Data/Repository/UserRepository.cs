using MyWallet.Data.Domain;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class UserRepository
    {
        private readonly IDocumentSession _session;

        public UserRepository(IDocumentSession session)
        {
            _session = session;
        }

        public void Save(User user)
        {
            _session.Store(user);
        }

        public User GetById(string id)
        {
            return _session.Load<User>(id);
        }

        public User GetByEmailAndPassword(string email, string password)
        {
            // TODO: create index
            var user = _session
                .Query<User>()
                .Include(u => u.MainContextId)
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            var mainContextId = _session.Load<Context>(user.MainContextId).Id;
            user.SetTheMainContext(mainContextId);

            return user;
        }

        public void UpdateMainContext(string userId, string mainContextId)
        {
            var user = _session.Query<User>()
                .Include(u => u.ContextIds)
                .FirstOrDefault(u => u.Id == userId);

            user.SetTheMainContext(mainContextId);

            var userContexts = _session.Load<Context>(user.ContextIds);

            foreach (var context in userContexts.Values)
            {
                context.IsMainContext = false;
                _session.Store(context);
            }

            _session.Store(user);
        }
    }
}
