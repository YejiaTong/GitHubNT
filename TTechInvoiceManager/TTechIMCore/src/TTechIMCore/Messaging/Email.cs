using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace TTechIMCore.Messaging
{
    public class Email
    {
        public static void SendEmail(string subject, string body, string fromEmail, string fromName, string toEmail, string toName, string bccEmail, string bccName)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress(toName, toEmail));
                if (String.IsNullOrEmpty(bccName) || String.IsNullOrEmpty(bccEmail))
                {

                }
                else
                {
                    message.Bcc.Add(new MailboxAddress(bccName, bccEmail));
                }
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    // Accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.sendgrid.net", 587, false);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("noah089736", "089736noahTYJ");

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new EmailHandlingException(ex.Message);
            }
        }
    }

    public class EmailHandlingException : Exception
    {
        public EmailHandlingException(string message)
            : base(String.Format("Failed to deliver system email:" + Environment.NewLine + message)) { }
    }
}
