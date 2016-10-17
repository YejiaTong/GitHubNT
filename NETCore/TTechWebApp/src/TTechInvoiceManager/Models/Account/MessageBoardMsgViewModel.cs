using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NTWebApp.Models.Account
{
    public class MessageBoardMsgViewModel
    {
        public int UserId { get; set; } = 0;

        [DisplayName("Message")]
        [DataType(DataType.Text)]
        [MessageBoardMsg]
        public string Message { get; set; } = String.Empty;

        [DisplayName("Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; } = DateTime.UtcNow;

        [Required]
        [DisplayName("Email")]
        [Email]
        public string Email { get; set; } = "non-reply@email.com";

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; } = "Anonymous";
    }
}
