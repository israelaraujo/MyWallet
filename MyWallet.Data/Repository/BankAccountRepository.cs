using MyWallet.Data.Domain;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class BankAccountRepository
    {
        private IDocumentSession _session;

        public BankAccountRepository(IDocumentSession session)
        {
            this._session = session;
        }

        public void Save(BankAccount bankAccount)
        {
            _session.Store(bankAccount);
        }

        public void Delete(BankAccount bankAccount)
        {
            _session.Delete(bankAccount);
        }

        public IEnumerable<BankAccount> GetByContextId(string contextId)
        {
            return _session
                .Query<BankAccount>()
                .Where(b => b.ContextId == contextId)
                .ToList();
        }

        public BankAccount GetById(string id)
        {
            return _session.Load<BankAccount>(id);
        }

        public IEnumerable<BankAccount> GetByName(IEnumerable<string> bankAccountNames, string contextId)
        {
            return _session
                .Query<BankAccount>()
                .Where(b => b.ContextId == contextId && bankAccountNames.Contains(b.Name))
                .ToList();
        }

        public IEnumerable<BankAccount> CreateIfNotExistsAndReturnAll(IEnumerable<string> newBankAccountsName, string contextId)
        {
            //var existentBankAccounts = GetByName(newBankAccountsName, contextId);

            //var allBankAccounts = new List<BankAccount>();
            //foreach (var bankAccountName in newBankAccountsName)
            //{
            //    var bankAccount = existentBankAccounts.FirstOrDefault(b => b.Name == bankAccountName);
            //    if (bankAccount == null)
            //    {
            //        var newBankAccount = new BankAccount
            //        {
            //            Name = bankAccountName,
            //            ContextId = contextId,
            //            CreationDate = DateTime.Now,
            //            OpeningBalance = 0
            //        };

            //        AddOrUpdate(newBankAccount);

            //        allBankAccounts.Add(newBankAccount);
            //    }
            //}

            //allBankAccounts.AddRange(existentBankAccounts);
            //return allBankAccounts;
            return null;
        }
    }
}
