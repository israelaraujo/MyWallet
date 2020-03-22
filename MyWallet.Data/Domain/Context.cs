namespace MyWallet.Data.Domain
{
    public class Context
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string CurrencyTypeId { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string CountryId { get; set; }
        public Country Country { get; set; }
        public bool IsMainContext { get; set; }
    }
}
