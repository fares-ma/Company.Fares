using System.ComponentModel.DataAnnotations;

namespace Company.Fares.PL.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
