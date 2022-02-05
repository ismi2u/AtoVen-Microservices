using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using AtoVen.API.Data;
using AtoVen.API.Controllers.AccountControl.Models;

namespace AtoVen.API.Controllers.AccountControl
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AtoVenDbContext _context;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, AtoVenDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }


        [HttpPost]
        [ActionName("CreateRole")]
        //[Authorize(Roles = "AtoVenAdmin, Admin")]
        public async Task<IActionResult> CreateRole([FromBody] RoleModelDTO model)
        {
            IdentityRole identityRole = new()
            {
                Name = model.RoleName
            };

            IdentityResult result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                return Ok(new { Status = "Success", Message = "New Role Created" });
            }

            return BadRequest(new { Status = "Failure", Message = "Role Creation Failed" });
        }



        [HttpGet]
        [ActionName("ListUsers")]

        public IActionResult ListUsers()
        {
            var users = _userManager.Users;

            return Ok(users);
        }

        [HttpGet]
        [ActionName("ListRoles")]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;

            return Ok(roles);
        }

        [HttpGet("{id}")]
        [ActionName("GetUserByUserId")]
        public async Task<IActionResult> GetUserByUserId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Conflict(new { Status = "Failure", Message = "User not Found" });
            }

            var rolenames = await _userManager.GetRolesAsync(user);


            List<string> roleids = new();

            foreach (string role in rolenames)
            {
                var tempRole = await _roleManager.FindByNameAsync(role);

                roleids.Add(tempRole.Id);
            }

            return Ok(new { user, roleids });
        }


        [HttpGet("{id}")]
        [ActionName("GetUsersByRoleId")]
        public async Task<IActionResult> GetUsersByRoleId(string id)
        {
            string rolName = _roleManager.Roles.Where(r => r.Id == id).FirstOrDefault().Name;

            //  List<string> UserIds =  context.UserRoles.Where(r => r.RoleId == id).Select(b => b.UserId).Distinct().ToList();

            List<UserByRoleDTO> ListUserByRole = new();

            var usersOfRole = await _userManager.GetUsersInRoleAsync(rolName);

            foreach (ApplicationUser user in usersOfRole)
            {
                UserByRoleDTO userByRole = new();

                userByRole.UserId = user.Id;
                userByRole.Email = user.Email;


                ListUserByRole.Add(userByRole);
            }

            return Ok(ListUserByRole);
        }

        [HttpGet("{id}")]
        [ActionName("GetRoleByRoleId")]
        public async Task<IActionResult> GetRoleByRoleId(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return Conflict(new { Status = "Failure", Message = "Role not Found" });
            }

            return Ok(role);
        }

        [HttpDelete]
        [ActionName("DeleteRole")]
        //[Authorize(Roles = "AtoVenAdmin, Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return Conflict(new { Status = "Failure", Message = "Role not Found" });
            }

            IdentityResult result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { Status = "Success", Message = "Role Deleted" });
            }


            return NotFound(new { Status = "Failure", Message = "Role Not Deleted!" });
        }

        [HttpDelete]
        [ActionName("DeleteUser")]
        //[Authorize(Roles = "AtoVenAdmin, Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Conflict(new { Status = "Failure", Message = "User not Found" });
            }

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { Status = "Success", Message = "User Deleted" });
            }

            return NotFound(new { Status = "Success", Message = "User Not Deleted!" });
        }



        [HttpPut]
        [ActionName("EditRole")]
        //[Authorize(Roles = "AtoVenAdmin, Admin")]
        public async Task<IActionResult> EditRole(EditRoleDTO model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            role.Name = model.RoleName;

            IdentityResult result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { Status = "Success", Message = "Role Updated" });
            }

            return NotFound(new { Status = "Failure", Message = "Role Not Updated" });
        }

        [HttpPost]
        [ActionName("CreateUser")]
        //[Authorize(Roles = "AtoVenAdmin, Admin")]
        public async Task<IActionResult> CreateUser(CreateUserDTO model)
        {
            var useremail = await _userManager.FindByEmailAsync(model.Email);

            if (useremail != null)
            {
                return Conflict(new { Status = "Failure", Message = "Email is already taken" });
            }

            //Creating a ApplicationUser object
            var NewUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                ApproverLevel = model.ApproverLevel,
                PasswordHash = model.Password

            };

            using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
            {

                IdentityResult UserAddResult = await _userManager.CreateAsync(NewUser, model.Password);

                var user = await _userManager.FindByEmailAsync(NewUser.Email);

                IdentityResult RoleAddresult = await _userManager.AddToRoleAsync(NewUser, model.Role);

                await AtoVenDbContextTransaction.CommitAsync();
            }


            return Ok(new { Status = "Success", Message = "User Created!" });

        }


        [HttpPut]
        [ActionName("EditUser")]
        //[Authorize(Roles = "AtoVenAdmin, Admin")]
        public async Task<IActionResult> EditUser(EditUserDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            user.UserName = model.Username;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { Status = "Success", Message = "User Updated!" });
            }


            return NotFound(new { Status = "Failure", Message = "User Not Updated!" });
        }


        [HttpPost]
        [ActionName("AssignRole")]
        //[Authorize(Roles = "AtoVenAdmin, Admin")]
        public async Task<IActionResult> AssignRole([FromBody] UserToRoleDTO Model)
        {

            string userId = Model.UserId;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            //Remove Existing Role.
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            //

            IdentityRole role = await _roleManager.FindByIdAsync(Model.RoleId);
            IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {

                return Ok(new { Status = "Success", Message = "Role Assigned!" });
            }

            return NotFound(new { Status = "Failure", Message = "Role Not Assigned!" });
        }

    }
}