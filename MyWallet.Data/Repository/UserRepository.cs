using MyWallet.Data.Domain;
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
            return _session.Query<User>().FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
