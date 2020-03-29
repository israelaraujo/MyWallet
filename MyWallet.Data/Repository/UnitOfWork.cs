using MyWallet.Data.RavenDB;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;

namespace MyWallet.Data.Repository
{
    public class UnitOfWork : IDisposable
    {
        private MyWalletDBContext _context;

        private readonly IDocumentStore _documentStore;
        private readonly IDocumentSession _session;

        public UnitOfWork()
        {
            _documentStore = RavenDocumentStoreHolder.Instance;
            _session = _documentStore.OpenSession();
        }

        private BankAccountRepository _bankAccountRepository;
        private CategoryRepository _categoryRepository;
        private ContextRepository _contextRepository;
        private CountryRepository _countryRepository;
        private CurrencyTypeRepository _currencyTypeRepository;
        private ExpenseRepository _expenseRepository;
        private IncomeRepository _incomeRepository;
        private UserRepository _userRepository;
        private ReportRepository _reportRepository;
        private bool _disposed;

        public BankAccountRepository BankAccountRepository => _bankAccountRepository ?? new BankAccountRepository(_session);
        public CategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_session);
        public ContextRepository ContextRepository => _contextRepository ?? new ContextRepository(_session);
        public CountryRepository CountryRepository => _countryRepository ?? new CountryRepository(_session);
        public CurrencyTypeRepository CurrencyTypeRepository => _currencyTypeRepository ?? new CurrencyTypeRepository(_session);
        public ExpenseRepository ExpenseRepository => _expenseRepository ?? new ExpenseRepository(_session);
        public IncomeRepository IncomeRepository => _incomeRepository ?? new IncomeRepository(_session);
        public UserRepository UserRepository => _userRepository ?? new UserRepository(_session);
        public ReportRepository ReportRepository => _reportRepository ?? new ReportRepository(_context);

        public void Commit()
        {
            _session.SaveChanges();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _session.Dispose();
                GC.SuppressFinalize(this);
            }
            _disposed = true;
        }
    }
}
