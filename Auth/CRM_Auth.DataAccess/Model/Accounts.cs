using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Auth.DataAccess.Model
{
    public class Accounts : IdentityUser<long>
    {
        public virtual ICollection<Account_Claims> Account_Claims { get; set; }
        public virtual ICollection<Account_Logins> Account_Logins { get; set; }
        public virtual ICollection<Account_Tokens> Account_Tokens { get; set; }
        public virtual ICollection<Account_Roles> Account_Roles { get; set; }
    }
}
