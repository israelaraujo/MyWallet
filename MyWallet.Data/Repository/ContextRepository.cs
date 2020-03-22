using MyWallet.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class ContextRepository
    {
        private MyWalletDBContext _context;

        public ContextRepository(MyWalletDBContext context)
        {
            _context = context;
        }

        public void Save(Context context)
        {
            _context.Context.Add(context);
        }

        public void Delete(Context context)
        {
            _context.Entry(context).State = System.Data.Entity.EntityState.Deleted;
        }

        public Context GetById(string id)
        {
            return _context.Context.Find(id);
        }

        public IEnumerable<Context> GetByUserId(string userId)
        {
            return _context.Context
                .Include(c => c.CurrencyType)
                .Include(c => c.Country)
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public void SetTheMainContextAsNonMain(string userId)
        {
            var mainContext = _context.Context.FirstOrDefault(c => c.UserId == userId && c.IsMainContext);
            mainContext.IsMainContext = false;
        }
    }
}
