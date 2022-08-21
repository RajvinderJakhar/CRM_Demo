using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Utility
{
    public sealed class AppSettings
    {
        public string No_Reply_Email { get; set; }
        public string No_Reply_Password { get; set; }
        public string SMTP_Host { get; set; }
        public string SMTP_Port { get; set; }
        public string EncSalt { get; set; }
        public string EncKey { get; set; }
        public string AuthKey { get; set; }

        public string SendGridKey { get; set; }
        public string SendGridEmail { get; set; }
        public string SendGridName { get; set; }

        public string TwilioAccountSID { get; set; }
        public string TwilioAuthToken { get; set; }
        public string Twilio_Phone { get; set; }

        public string API_Scheme { get; set; }

        public string AuthAPI_BaseUrl { get; set; }
        public string EmployerAPI_BaseUrl { get; set; }
        public string CandidateAPI_BaseUrl { get; set; }
    }
}
