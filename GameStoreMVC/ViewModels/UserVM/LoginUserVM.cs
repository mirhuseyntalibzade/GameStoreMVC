using System.ComponentModel.DataAnnotations;

namespace GameStoreMVC.ViewModels.UserVM
{
    public class LoginUserVM
    {
        [Required]
        [Display(Prompt = "Email")]
        public string EmailOrUserName { get; set; }
        
        [Required]
        [Display(Prompt = "Password")]
        public string Password { get; set; }
    }
}
