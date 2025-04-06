using Company.Fares.PL.Settings;
using Company.Fares.PL.Helpers;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
//using System.Net.Mail;

namespace Company.Fares.PL.Helpers
{
    public class MailService(IOptions<MailSettings> _options) : IMailService
    {
        #region Old
        //private readonly MailSettings _options;

        //public MailService(IOptions<MailSettings> options)
        //{
        //    _options = options.Value;
        //} 
        #endregion

        public void SendEmail(Email email)
        {
            // Build Message

            var mail = new MimeMessage();

            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();

            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // Establish connection

            using var smpt = new SmtpClient();

            smpt.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);

            smpt.Authenticate(_options.Value.Email, _options.Value.Password);


            //  Send Message
            smpt.Send(mail);
        }
    }
}

    
