using System.ComponentModel.DataAnnotations;

namespace ConsumingJWT_WebApi_Authentication_.Models
{
    public class RegisterViewModels
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}