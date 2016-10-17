using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Email or Username")]
        [LoginId]
        public string LoginId { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember Me?")]
        public bool RememberMe { get; set; }
    }
}
