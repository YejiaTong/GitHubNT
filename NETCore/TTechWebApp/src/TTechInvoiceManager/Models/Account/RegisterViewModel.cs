using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Username")]
        [UserName]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [Email]
        public string Email { get; set; }
    }
}
