using System.ComponentModel.DataAnnotations;

namespace AtoVen.API.Controllers.AccountControl.Models
{
    public class ForgotPasswordDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]

        public string email { get; set; }
    }
}
