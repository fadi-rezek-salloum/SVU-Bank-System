using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
        public IFormFile ImagePath { get; set; }
    }
}