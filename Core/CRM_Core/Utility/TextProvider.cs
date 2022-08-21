using CRM_Core.ViewModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CRM_Core.Utility
{
    public sealed class TextProvider : ITextProvider
    {
        #region Fields
        private readonly AppSettings _appSettings;
        #endregion
        #region Contructors
        public TextProvider(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        #endregion

        #region Send Text
        public async Task<ApiResponse<MessageResourceVewModel>> SendSMS(string toPhone, string smsText)
        {
            var result = new ApiResponse<MessageResourceVewModel>();
            if (!String.IsNullOrEmpty(toPhone) && !String.IsNullOrEmpty(smsText))
            {
                TwilioClient.Init(_appSettings.TwilioAccountSID, _appSettings.TwilioAuthToken);

                var to = new PhoneNumber(toPhone);
                var fromPhone = new PhoneNumber(_appSettings.Twilio_Phone);
                try
                {
                    var message = await MessageResource.CreateAsync(
                        to,
                        from: fromPhone,
                        body: smsText,
                        statusCallback: new Uri("https://fillcontracts.com/support/receive/message"));

                    if (message != null && !String.IsNullOrEmpty(message.Sid))
                    {
                        result.Data = new MessageResourceVewModel()
                        {
                            AccountSid = message.AccountSid,
                            SmsSid = message.Sid,
                            SmsStatus = message.Status.ToString(),
                            From_Phone_Number = _appSettings.Twilio_Phone,
                            IsSMS = true,
                            IsIncoming = false,
                            TO_Phone_Number = toPhone,
                            Body = smsText
                        };
                    }
                    else
                        result.Data = null;
                }
                catch (Exception ex)
                {
                    result.AddError(ex.Message);
                }


            }

            return result;
        }
        #endregion
    }
}
