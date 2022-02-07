using System.ComponentModel.DataAnnotations;

namespace DataService.AccountControl.Models
{
    public class ForgotPasswordDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]

        public string email { get; set; }
    }
}
