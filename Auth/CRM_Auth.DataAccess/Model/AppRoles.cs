using Microsoft.AspNetCore.Identity;

namespace CRM_Auth.DataAccess.Model
{
    public class AppRoles : IdentityRole<long>
    {
        public AppRoles() { }
        public AppRoles(string name)
        {
            Name = name;
        }


        public string? Description { get; set; }

        public virtual ICollection<Account_Roles> Account_Roles { get; set; }
        public virtual ICollection<Account_RoleClaims> Account_RoleClaims { get; set; }
    }

    public class UserRoles
    {
        public const string Administrator = "Administrator";
        public const string User = "User";
        public const string Employer = "Employer";
    }
}
