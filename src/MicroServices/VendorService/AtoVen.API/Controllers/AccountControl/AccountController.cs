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
using AtoVen.API.Controllers.AccountControl.Models;

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


        [HttpPost]
        [ActionName("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            //check if employee-id is already registered



            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.email);

                //bool isUserConfirmed = await userManager.IsEmailConfirmedAsync(user);
                //if (user != null && isUserConfirmed)

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var passwordResetlink= Url.Action("ResetPassword", "Account", new { email = model.email, token = token, Request.Scheme });

                    //return Ok(passwordResetLink);
                    token = token.Replace("+", "^^^");
                    var receiverEmail = model.email;
                    string subject = "Password Reset Link";
                    string content = "Please click the below Password Reset Link to reset your password:" + Environment.NewLine +
                                        "https://atoven.com/change-password?token=" + token + "&email=" + model.email;

                    //"<a href=\"https://atoven.com/change-password?token=" + token + "&email=" + model.email + "\">";
                    var messagemail = new Message(new string[] { receiverEmail }, subject, content);

                    await _emailSender.SendEmailAsync(messagemail);

                }

                return  Ok(new { Status = "Success", Message = "Password Reset Email Sent Successfully!" });
            }
            return Conflict(new { Status = "Failure", Message = "UserId is Invalid!" });
        }




        [HttpPost]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.email);

                if (user != null)
                {

                    model.Token = model.Token.Replace("^^^", "+");
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        var receiverEmail = model.email;
                        string subject = "Password Changed";
                        string content = "Your new Password is:" + model.Password;
                        var messagemail = new Message(new string[] { receiverEmail }, subject, content);

                        await _emailSender.SendEmailAsync(messagemail);
                        return Ok(new { Status = "Success", Message = "Your Password has been reset!" });
                    }

                    List<object> errResp = new();
                    foreach (var error in result.Errors)
                    {
                        errResp.Add(error.Description);
                    }
                    return Ok(errResp);
                }

                return Conflict(new { Status = "Failure", Message = "User Not Found!" });

            }

            return Conflict(new { Status = "Failure", Message = "Model state is invalid" });

        }
    }
}
