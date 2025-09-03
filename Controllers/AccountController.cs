using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IEmailSender emailSender, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDTO.username,
                    Email = registerDTO.email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDTO.password ?? "");

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        var emailtoken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GenerateEmailConfirmationTokenAsync(appUser)));
                        var emaillink = $"{_config["ClientURL"]}/confirm-email?userId={appUser.Id}&token={emailtoken}";

                        await _emailSender.SendEmailAsync(appUser.Email ?? "", "Viktor Marchenko email verification.", $"Please, verify your email by the following link: {emaillink}");

                        return Ok(
                            new NewUserDTO
                            {
                                username = appUser.UserName,
                                email = appUser.Email,
                                roles = (await _userManager.GetRolesAsync(appUser)).ToList(),
                                token = await _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => (x.UserName ?? "").ToLower() == loginDTO.username.ToLower());

            if (user == null)
            {
                return Unauthorized("Username not found or password incorrect.");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized("Email not confirmed.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Username not found or password incorrect.");
            }

            return Ok
            (
                new NewUserDTO
                {
                    username = user.UserName,
                    email = user.Email,
                    roles = (await _userManager.GetRolesAsync(user)).ToList(),
                    token = await _tokenService.CreateToken(user)
                }
            );
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var decodedtoken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedtoken);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid or expired token.");
            }

            return Ok("Email confirmed.");
        }

        [Authorize]
        [HttpGet("confirm-user")]
        public IActionResult ConfirmUser()
        {
            return Ok("Authorized");
        }
    }
}