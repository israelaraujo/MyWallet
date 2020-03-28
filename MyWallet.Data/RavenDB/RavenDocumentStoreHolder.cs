using MyWallet.Data.DBInitializer;
using MyWallet.Data.Repository.Index.Expense;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System;

namespace MyWallet.Data.RavenDB
{
    public class RavenDocumentStoreHolder /*: IRavenDocumentStoreHolder*/
    {
        private static IDocumentStore _instance;

        public static IDocumentStore Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException($"{nameof(IDocumentStore)} has not been initialized.");

                return _instance;
            }
        }

        public static IDocumentStore InitializeDB()
        {
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "MyWallet",
                Conventions = new DocumentConventions
                {
                    CustomizeJsonSerializer = serializer => serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    IdentityPartsSeparator = "-"
                }
            };

            documentStore.Initialize();
            RegisterIndexes(documentStore);
            EnsureDatabaseExists(documentStore);
            MyWalletInitializer.SeedInitialData(documentStore);

            _instance = documentStore;
            return _instance;
        }

        private static void EnsureDatabaseExists(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database = database ?? store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                store.Maintenance.ForDatabase(database).Send(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException ex)
            {
                if (createDatabaseIfNotExists == false) throw;

                try
                {
                    store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
                }
                catch (ConcurrencyException concurrencyEx)
                {
                    // The database was already created before calling CreateDatabaseOperation
                }
            }
        }

        private static void RegisterIndexes(DocumentStore documentStore)
        {
            new Expense_ByContextId().Execute(documentStore);
        }
    }
}
