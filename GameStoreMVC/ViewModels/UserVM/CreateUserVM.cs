using System.ComponentModel.DataAnnotations;

namespace GameStoreMVC.ViewModels.UserVM
{
    public class CreateUserVM
    {
        [Required]
        [Display(Prompt = "Firstname")]
        public string Name { get; set; }

        [Required]
        [Display(Prompt = "Lastname")]
        public string Surname { get; set; }

        [Required]
        [Display(Prompt = "Username")]
        public string UserName { get; set; }
        
        [Required]
        [Display(Prompt = "Email")]
        public string Email { get; set; }
        
        [Required]
        [Display(Prompt = "Password")]
        public string Password { get; set; }
        
        [Required]
        [Display(Prompt = "Your password")]
        public string ConfirmPassword { get; set; }
    }
}
