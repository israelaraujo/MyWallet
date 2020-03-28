using MyWallet.Data.Domain;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace MyWallet.Data.Repository.Index
{
    public class Currency_GetSymbolByContextId : AbstractIndexCreationTask<Context, Currency_GetSymbolByContextId.Result>
    {
        public class Result
        {
            public string ContextId { get; set; }
            public string CurrencySymbol { get; set; }
        }

        public Currency_GetSymbolByContextId()
        {
            Map = contexts => from context in contexts
                              select new
                              {
                                  ContextId = context.Id,
                                  CurrencySymbol = LoadDocument<CurrencyType>(context.CurrencyTypeId).Symbol
                              };

            Reduce = results => from result in results
                                group result by new
                                {
                                    result.ContextId,
                                    result.CurrencySymbol,
                                }
                                into g
                                select new
                                {
                                    g.Key.ContextId,
                                    g.Key.CurrencySymbol,
                                };
        }
    }
}
