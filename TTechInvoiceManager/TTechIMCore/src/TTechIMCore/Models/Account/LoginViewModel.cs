using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - Login ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.Account
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
