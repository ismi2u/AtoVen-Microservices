using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoVen.API.Data;
using AtoVen.API.Entities;
using Microsoft.AspNetCore.Identity;
using AtoVen.API.Entities.UserLoginEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using AtoVen.API.Entities.ValiationResultEntities;
using System.Text.Json;
using EmailSendService;

namespace AtoVen.API.Controllers.AccountControl
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AtovenDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(AtovenDbContext context, IEmailSender emailSender,
                                UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        [HttpPost]
        [ActionName("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<Company>> AccountLogin(Login login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);

            if (result.Succeeded)
            {
                var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey12323232"));

                var signingcredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

                var modeluser = await _userManager.FindByEmailAsync(login.Email);
                var userroles = await _userManager.GetRolesAsync(modeluser);


                //add claims
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, modeluser.UserName),
                 new Claim(ClaimTypes.Email, login.Email)


                };
                //add all roles belonging to the user
                foreach (var role in userroles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var tokenOptions = new JwtSecurityToken(

                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddHours(5),
                     signingCredentials: signingcredentials
                    );


                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);


                return Ok(new { Token = tokenString, Role = userroles, Email = login.Email });

            }
            return Unauthorized(new { Status = "Failure", Message = "Incorrect User-Id/Password!" });
        }
        [HttpPost]
        [ActionName("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> AccountLogout()
        {
            await _signInManager.SignOutAsync();

            return Ok(new { Status = "Success", Message = "Logged Out Successfully!" });
        }
    }
}
