

using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public class User
    {
        // [Required]
        // public string FirstName { get; set; }
        // [Required]
        // public string LastName { get; set; }
        // [Required]
        // [DataType(DataType.PhoneNumber)]
        // public string Phone { get; set; }
        // [Required]
        // public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { set; get; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { set; get; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords didn't match!")]
        public string ConfirmPassword { get; set; }
    }
}