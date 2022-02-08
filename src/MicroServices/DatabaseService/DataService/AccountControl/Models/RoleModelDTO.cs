using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataService.AccountControl.Models
{
    public class RoleModelDTO
    {
        public string RoleName { get; set; }
    }

    public class RoleToUserSearchDTO
    {
        public string RoleId { get; set; }
    }


    public class UserToRoleDTO
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }


    }


    public class UserByRoleDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string StatusType { get; set; }
        public string Role { get; set; }

    }
    public class EditRoleDTO
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
    }

    public class CreateUserDTO
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int ApproverLevel { get; set; }
        public string Role { get; set; }

    }

    public class EditUserDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int ApproverLevel { get; set; }
        public string Role { get; set; }

    }
}
