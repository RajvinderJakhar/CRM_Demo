using Microsoft.AspNetCore.Identity;

namespace CRM_Auth.DataAccess.Model
{
    public class Account_Roles : IdentityUserRole<long>
    {
        public virtual Accounts Accounts { get; set; }
        public virtual AppRoles AppRoles { get; set; }
    }
}
