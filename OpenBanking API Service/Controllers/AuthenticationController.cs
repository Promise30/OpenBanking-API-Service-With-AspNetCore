using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenBanking_API_Service.Dtos;
using OpenBanking_API_Service.Service.Constants;
using OpenBanking_API_Service.Service.Interface;
using System.Net;

namespace OpenBanking_API_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;

        public AuthenticationController(IUserService userService,
                                        IEmailService emailService,
                                        IAccountService accountService)
        {
            _userService = userService;
            _emailService = emailService;
            _accountService = accountService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }
            var tokenResponse = await _userService.RegisterNewUser(registerUserDto);

            if (tokenResponse.Data != null)
            {
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token = tokenResponse.Data, email = registerUserDto.Email }, Request.Scheme);
                var message = new EmailMessage(to: new string[] { registerUserDto.Email }, subject: "Email Confirmation", body: confirmationLink);
                var responseMessage = _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK, tokenResponse);
            }
            return StatusCode(StatusCodes.Status400BadRequest, tokenResponse);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var result = await _userService.UserEmailConfirmation(token, email);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : BadRequest(result);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }

            var result = await _userService.LoginUserAsync(loginDto);

            if (result.Data != null)
            {
                var otpToken = result.Data.Token;
                var message = new EmailMessage(new string[] { loginDto.Email }, "OTP Confirmation Code", otpToken);
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK, result);

            }
            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        [HttpPost("login-2FA")]
        public async Task<IActionResult> Login2FA(TwoFactorModel twoFactorModel)
        {
            var result = await _userService.LoginUserWithTwoFactorEnabled(twoFactorModel);
            if (result.Data != null)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return BadRequest(result);
        }
        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(model);
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, "Validation Failed", ModelState));
            }
            var result = await _userService.PasswordResetAsync(resetPassword);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : BadRequest(result);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            var result = await _userService.ForgotPasswordRequest(forgotPassword);
            if (result.Data != null)
            {
                var resetPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token = result.Data, email = forgotPassword.Email }, Request.Scheme);
                var message = new EmailMessage(to: new string[] { forgotPassword.Email }, subject: "Password Reset Confirmation Link", body: resetPasswordLink);
                var responseMessage = _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK, result);
            }

            return StatusCode(StatusCodes.Status400BadRequest, result);


        }





    }



}
