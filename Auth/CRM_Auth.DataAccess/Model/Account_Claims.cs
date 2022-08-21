using Microsoft.AspNetCore.Identity;

namespace CRM_Auth.DataAccess.Model
{
    public class Account_Claims : IdentityUserClaim<long>
    {
        public virtual Accounts Accounts { get; set; }
    }
}
