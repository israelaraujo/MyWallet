using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWallet.Data.Domain
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] Photo { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Context> Contexts { get; set; }

        public User()
        {
            Contexts = new List<Context>();
        }

        public Context GetTheMainContext()
        {
            return Contexts.FirstOrDefault(c => c.IsMainContext);
        }

        public string GetTheMainContextId()
        {
            return GetTheMainContext().Id;
        }

        public void SetTheMainContext(Context context)
        {
            SetAllContextsToNonMainContexts();

            var contextStored = Contexts.FirstOrDefault(c => c.Id == context.Id);
            if (contextStored != null)
            {
                contextStored.IsMainContext = true;
                return;
            }

            context.IsMainContext = true;
            AddContext(context);
        }

        private void SetAllContextsToNonMainContexts()
        {
            foreach (var c in Contexts)
                c.IsMainContext = false;
        }

        private void AddContext(Context context)
        {
            if (Contexts.Any(c => c.Id == context.Id)) return;

            Contexts.Add(context);
        }
    }
}
