using System.ComponentModel.DataAnnotations;

namespace ConsumingJWT_WebApi_Authentication_.Models
{
    public class LoginViewModels
    {
        [Required]
        public string UserName { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
