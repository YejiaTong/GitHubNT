﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - Password ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.Home
{
    public class PasswordViewModel
    {
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Password]
        public string Password { get; set; }

        [Required]
        [DisplayName("Re-Password")]
        [DataType(DataType.Password)]
        [Password]
        public string RetryPassword { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
