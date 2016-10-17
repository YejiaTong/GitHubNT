using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.Home
{
    public class AccountViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string Gender { get; set; }

        public int IsActive { get; set; }

        public string Password { get; set; }

        public string SecurityToken { get; set; }

        public string Description { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string DBInstance { get; set; }

        public string RetryPassword { get; set; }
    }
}
