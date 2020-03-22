namespace MyWallet.Data.Domain
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ContextId { get; set; }
        public Context Context { get; set; }
    }
}
