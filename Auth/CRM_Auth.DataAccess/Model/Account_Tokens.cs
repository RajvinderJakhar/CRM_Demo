using Microsoft.AspNetCore.Identity;

namespace CRM_Auth.DataAccess.Model
{
    public class Account_Tokens : IdentityUserToken<long>
    {
        public virtual Accounts Accounts { get; set; }
    }
}
