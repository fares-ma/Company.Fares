using Company.Fares.DAL.Models.Sms;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Fares.PL.Helpers
{
    public interface ITwilioService
    {
         MessageResource SendSms( Sms sms);
    }
}
