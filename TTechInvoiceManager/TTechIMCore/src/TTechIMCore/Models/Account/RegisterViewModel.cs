using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - Registration ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.Account
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
