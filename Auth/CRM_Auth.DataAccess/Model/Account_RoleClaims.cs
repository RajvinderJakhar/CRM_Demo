using Microsoft.AspNetCore.Identity;

namespace CRM_Auth.DataAccess.Model
{
    public class Account_RoleClaims : IdentityRoleClaim<long>
    {
        public virtual AppRoles AppRoles { get; set; }
    }
}
