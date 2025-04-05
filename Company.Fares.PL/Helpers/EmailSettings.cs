using System.Net;
using System.Net.Mail;

namespace Company.Fares.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email    )
        {

			try
			{
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("farsagwa9gmail.com", "qdxgknhqpbaiqjha");
                client.Send("farsagwa9gmail.com", email.To, email.Subject, email.Body);


                return true;

            }
			catch (Exception e)
			{
                return false;


			}
        }
    }
}
