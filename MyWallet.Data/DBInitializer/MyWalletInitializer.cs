using MyWallet.Data.Domain;
using Raven.Client.Documents;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace MyWallet.Data.DBInitializer
{
    public class MyWalletInitializer : CreateDatabaseIfNotExists<MyWalletDBContext>
    {
        public static void SeedInitialData(DocumentStore documentStore)
        {
            //TODO: ...
        }
    }
}
