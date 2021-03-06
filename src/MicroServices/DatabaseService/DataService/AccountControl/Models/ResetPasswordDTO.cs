using System.ComponentModel.DataAnnotations;
namespace DataService.AccountControl.Models
{
    public class ResetPasswordDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
