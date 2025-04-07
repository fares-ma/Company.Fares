using Company.Fares.DAL.Models.Sms;
using Company.Fares.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Fares.PL.Helpers
{
    public class TwilioService(IOptions<TwilioSettings> options) : ITwilioService
    {
        public MessageResource SendSms(Sms sms)
        {
            // Intialize Connection

            TwilioClient.Init(options.Value.AccountSID, options.Value.AuthToken);
           

            // Build Message

            var message = MessageResource.Create(
                body: sms.Body,
                to: sms.To,
                from: options.Value.PhoneNumber
                //from:new Twilio.Types.PhoneNumber("+201143941265")
                );

            // return Message
            return message;
        }

    }
}
