using MyWallet.Data.Domain;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class CountryRepository
    {
        private IDocumentSession _session;

        public CountryRepository(IDocumentSession session)
        {
            _session = session;
        }

        public IEnumerable<Country> GetAll()
        {
            return _session.Query<Country>().ToList();
        }
    }
}
