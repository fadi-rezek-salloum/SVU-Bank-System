using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class OfferViewModel
    {
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public IFormFile ImagePath { get; set; }
    }
}