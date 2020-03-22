using MyWallet.Data.Domain;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWallet.Data.Repository
{
    public class CategoryRepository
    {
        private IDocumentSession session;

        public CategoryRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public void Save(Category category)
        {
            session.Store(category);
        }

        public void Delete(string categoryId)
        {
            session.Delete(categoryId);
        }

        public IEnumerable<Category> GetByName(IEnumerable<string> categories, string contextId)
        {
            //var query = _context.Category.Where(c => c.ContextId == contextId && categories.Contains(c.Name));
            //return query.ToList();
            return null;
        }

        public IEnumerable<Category> GetByContextId(string contextId)
        {
            return session.Query<Category>().Where(c => c.ContextId == contextId);
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
            //foreach (var categoryName in newCategoriesName)
            //{
            //    var category = existentCategories.FirstOrDefault(c => c.Name == categoryName);
            //    if (category == null)
            //    {
            //        var newCategory = new Category { Name = categoryName, ContextId = contextId };
            //        AddOrUpdate(newCategory);
            //        allCategories.Add(newCategory);
            //    }
            //}

            //allCategories.AddRange(existentCategories);

            return allCategories;
        }

        public Category GetById(string id)
        {
            return session.Load<Category>(id);
        }
    }
}
