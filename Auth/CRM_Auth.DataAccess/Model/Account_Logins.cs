using Microsoft.AspNetCore.Identity;

namespace CRM_Auth.DataAccess.Model
{
    public class Account_Logins : IdentityUserLogin<long>
    {
        public virtual Accounts Accounts { get; set; }
    }
}
