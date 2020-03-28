using MyWallet.Common.Extensions;
using MyWallet.Common.Util;
using MyWallet.Data.Domain;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Data.Entity;
using System.Linq;

namespace MyWallet.Data.DBInitializer
{
    public class MyWalletInitializer : CreateDatabaseIfNotExists<MyWalletDBContext>
    {
        public static void SeedInitialData(IDocumentStore documentStore)
        {
            using (var session = documentStore.OpenSession())
            {
                var hasAnyUser = session.Query<User>().Any();

                if (hasAnyUser)
                    return;

                // Currency
                var euroCurrency = new CurrencyType() { Name = "Euro", Symbol = "€" };

                session.Store(euroCurrency);
                session.Store(new CurrencyType() { Name = "Dollar", Symbol = "$" });
                session.Store(new CurrencyType() { Name = "Real", Symbol = "R$" });

                // Country
                var portugal = new Country { Name = "Portugal" };

                session.Store(portugal);
                session.Store(new Country { Name = "Brazil" });
                session.Store(new Country { Name = "France" });

                // Admin user
                var adminUser = BindAdminUser(euroCurrency.Id, portugal.Id, session);

                session.Store(adminUser);
                session.SaveChanges();
            }
        }

        private static User BindAdminUser(string currencyId, string countryId, IDocumentSession session)
        {
            var user = new User();
            user.Name = "admin";
            user.LastName = "admin";
            user.Email = "admin@gmail.com";
            user.Password = CryptographyUtil.Encrypt("123456");
            user.CreationDate = DateTime.Now;

            session.Store(user);

            // Context
            var mainContext = new Context()
            {
                UserId = user.Id,
                IsMainContext = true,
                Name = "My Finances",
                CountryId = countryId,
                CurrencyTypeId = currencyId
            };
            session.Store(mainContext);

            // Bank Account
            var bankAccount = new BankAccount()
            {
                Name = "Default",
                ContextId = mainContext.Id,
                CreationDate = DateTime.Now,
                OpeningBalance = 10000
            };
            session.Store(bankAccount);

            var category = new Category()
            {
                Name = "Default",
                ContextId = mainContext.Id
            };
            session.Store(category);

            return user;
        }
    }
}
