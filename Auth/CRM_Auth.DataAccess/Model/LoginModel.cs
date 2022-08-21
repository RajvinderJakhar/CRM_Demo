using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Auth.DataAccess.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100)]
        [RegularExpression(@"^([\w\.\-\+]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email and password.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} field is required")]
        [StringLength(20, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.{4,})(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[~#!@$*%^:_|-])[0-9a-zA-Z~#!@$*%^:_|-]*$", ErrorMessage = "Please enter a valid email and password!")]
        public string Password { get; set; }

    }

    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public int TokenExpire { get; set; }
        public string TokenType { get; set; }

    }


    public enum JWT_TokenTypes
    {
        bearer
    }
}
