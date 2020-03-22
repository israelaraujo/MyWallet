using System.ComponentModel.DataAnnotations;

namespace MyWallet.Web.ViewModels.Category
{
    public class CategoryViewModel
    {
        public string Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}