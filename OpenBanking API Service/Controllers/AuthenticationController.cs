using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenBanking_API_Service.Dtos;
using OpenBanking_API_Service.Service.Interface;
using OpenBanking_API_Service_Common.Library.Models;

namespace OpenBanking_API_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public AuthenticationController(ILogger<AuthenticationController> logger,
                                        IUserService userService,
                                        IEmailService emailService,
                                        UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var tokenResponse = await _userService.CreateUserWithToken(registerUserDto);
            if (tokenResponse.Data != null)
            {
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token = tokenResponse.Data, email = registerUserDto.Email }, Request.Scheme);
                var message = new EmailMessage(to: new string[] { registerUserDto.Email }, subject: "Email Confirmation", body: confirmationLink);
                var responseMessage = _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK,
                        new Response { IsSuccess = true, Message = $"{tokenResponse.StatusMessage} {responseMessage}" });


            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = tokenResponse.StatusMessage, IsSuccess = false });
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Message = "Email verified successfully", IsSuccess = true });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Message = "Email verification failed. User does not exist.", IsSuccess = false });

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var loginOtpResponse = await _userService.GetOtpByLoginAsync(loginDto);
            if (loginOtpResponse.Data != null)
            {
                var user = loginOtpResponse.Data.User;
                if (user.TwoFactorEnabled)
                {
                    var token = loginOtpResponse.Data?.Token;
                    var message = new EmailMessage(new string[] { user.Email }, "OTP Confirmation code", token);
                    _emailService.SendEmail(message);

                    return StatusCode(StatusCodes.Status200OK, new Response { IsSuccess = true, Message = loginOtpResponse.StatusMessage });
                }
                if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    var response = await _userService.GetJwtTokenAsync(user);
                    return StatusCode(StatusCodes.Status200OK, response);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, new Response { IsSuccess = false, Message = loginOtpResponse.StatusMessage });
        }

        [HttpPost("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string email)
        {
            var jwt = await _userService.LogInUserWIthOtpAsync(code, email);
            if (jwt.Data != null)
            {
                return StatusCode(StatusCodes.Status200OK, jwt.Data);
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Message = jwt.StatusMessage, IsSuccess = false });
        }
    }

}
