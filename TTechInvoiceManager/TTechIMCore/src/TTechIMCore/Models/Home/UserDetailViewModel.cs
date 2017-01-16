using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/*
 * TTech IM - App Model Layer - UserDetail ViewModel
 * Data Annotations
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Models.Home
{
    public class UserDetailViewModel
    {
        public int UserId { get; set; }

        [DisplayName("Username")]
        public string UserName { get; set; }

        [DisplayName("Email Address")]
        public string Email { get; set; }

        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [DisplayName("Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [DisplayName("Postal Code")]
        [PostalCode]
        public string PostalCode { get; set; }

        [DisplayName("Gender")]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
