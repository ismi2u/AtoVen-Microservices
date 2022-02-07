using Microsoft.AspNetCore.Identity;
namespace DataService.AccountControl.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int ApproverLevel { get; set; }
    }
}
