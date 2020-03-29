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
        public string MainContextId { get; set; }
        public ICollection<string> ContextIds { get; set; }

        // TODO: because entity framework - remove later
        public ICollection<Context> Contexts { get; set; }

        public User()
        {
            ContextIds = new List<string>();
        }

        public string GetTheMainContextId()
        {
            return MainContextId;
        }

        public void SetTheMainContext(string contextId)
        {
            if (!ContextIds.Any(c => c == contextId))
                ContextIds.Add(contextId);

            MainContextId = contextId;
        }

        public void AddContext(string contextId)
        {
            if (ContextIds.Any(c => c == contextId)) return;

            ContextIds.Add(contextId);
        }
    }
}
