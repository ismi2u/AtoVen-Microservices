using Microsoft.AspNetCore.Identity;
namespace AtoVen.API.Controllers.AccountControl.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int ApproverLevel { get; set; }
    }
}
