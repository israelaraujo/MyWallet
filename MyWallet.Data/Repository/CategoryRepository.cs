using MyWallet.Data.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;

namespace MyWallet.Data.Repository
{
    public class CategoryRepository
    {
        private MyWalletDBContext _context;

        public CategoryRepository(MyWalletDBContext context)
        {
            _context = context;
        }

        public void Save(Category category)
        {
            _context.Category.Add(category);
        }

        public void Delete(Category category)
        {
            _context.Entry(category).State = EntityState.Deleted;
        }

        public Category GetById(string id)
        {
            return _context.Category.Find(id);
        }

        public IEnumerable<Category> GetByName(IEnumerable<string> categories, string contextId)
        {
            var query = _context.Category.Where(c => c.ContextId == contextId && categories.Contains(c.Name));
            return query.ToList();
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Category.ToList();
        }

        public IEnumerable<Category> GetByContextId(string contextId)
        {
            return _context.Category.Where(c => c.ContextId == contextId).ToList();
        }

        public IEnumerable<Category> GetStandardCategories()
        {
            return new List<Category>
            {
                new Category{ Name = "Food" },
                new Category{ Name = "Groceries" },
                new Category{ Name = "Shopping" },
                new Category{ Name = "Transportation" },
                new Category{ Name = "Entertainment" },
                new Category{ Name = "Education" },
                new Category{ Name = "Health" },
                new Category{ Name = "Home" },
                new Category{ Name = "Debts" },
                new Category{ Name = "Salary" },
                new Category{ Name = "Other" }
            };
        }

        public IEnumerable<Category> CreateIfNotExistsAndReturnAll(IEnumerable<string> newCategoriesName, string contextId)
        {
            var existentCategories = GetByName(newCategoriesName, contextId);

            var allCategories = new List<Category>();
            foreach (var categoryName in newCategoriesName)
            {
                var category = existentCategories.FirstOrDefault(c => c.Name == categoryName);
                if (category == null)
                {
                    var newCategory = new Category { Name = categoryName, ContextId = contextId };
                    Save(newCategory);
                    allCategories.Add(newCategory);
                }
            }

            allCategories.AddRange(existentCategories);

            return allCategories;
        }
    }
}
