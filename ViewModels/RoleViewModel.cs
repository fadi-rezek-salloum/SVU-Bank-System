using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}