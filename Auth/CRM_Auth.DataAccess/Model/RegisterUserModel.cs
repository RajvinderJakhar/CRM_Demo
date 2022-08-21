using System.ComponentModel.DataAnnotations;

namespace CRM_Auth.DataAccess.Model
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password don't match.")]
        public string ConfirmPassword { get; set; }

    }

}
