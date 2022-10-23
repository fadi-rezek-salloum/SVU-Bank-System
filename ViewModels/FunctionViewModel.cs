using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class FunctionViewModel
    {
        public string Name { get; set; }
        [Required]
        public string Icon { get; set; }
        [Required]
        public string Body { get; set; }
        public IFormFile ImagePath { get; set; }
    }
}