using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyWallet.Web.ViewModels.Context
{
    public class ContextViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Currency")]
        public string CurrencyTypeId { get; set; }

        [Required]
        [DisplayName("Country")]
        public string CountryId { get; set; }

        [DisplayName("Main Context")]
        public bool IsMainContext { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string CountryName { get; set; }
    }
}