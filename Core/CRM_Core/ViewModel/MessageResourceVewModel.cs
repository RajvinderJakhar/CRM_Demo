using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.ViewModel
{
    public class MessageResourceVewModel
    {
        public string AccountSid { get; set; }
        public string SmsSid { get; set; }
        public string TO_Phone_Number { get; set; }
        public string From_Phone_Number { get; set; }
        public string FromCountry { get; set; }
        public string SmsStatus { get; set; }
        public string Body { get; set; }
        public bool IsIncoming { get; set; }
        public bool IsSMS { get; set; }
    }
}
