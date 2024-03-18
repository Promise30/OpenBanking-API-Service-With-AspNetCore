using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests;
using OpenBanking_API_Service.Dtos.AuthenticationDtos.Responses;
using OpenBanking_API_Service.Routes;
using OpenBanking_API_Service.Service.Constants;
using OpenBanking_API_Service.Service.Interface;
using System.Net;
using System.Net.Mime;

namespace OpenBanking_API_Service.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public AuthenticationController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        /// <summary>
        /// register a new user to use the service
        /// </summary>
        /// <param name="registerUserDto"></param>
        /// <returns>An email confirmation link</returns>
        /// <response code="200">Account was  created successful</response>

        /// <response code="400">If model is invalid or username already exists</response>
        /// <response code="500">If something went wrong with the server</response>
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost]
        [Route(AuthenticationRoutes.RegisterUser)]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }

            var response = await _userService.RegisterNewUser(registerUserDto);

            if (response.StatusCode == HttpStatusCode.OK)
            {

                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token = response.Data, email = registerUserDto.Email }, Request.Scheme);
                var message = new EmailMessage(to: new string[] { registerUserDto.Email }, subject: "Email Confirmation", body: confirmationLink);
                var responseMessage = _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK, response);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        /// <summary>
        /// confirm a newly registered user email
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <returns>a success confirmation message</returns>
        /// <response code="200">If email confirmation was successful</response>
        [ProducesResponseType(200)]
        [HttpGet]
        [Route(AuthenticationRoutes.ConfirmUserEmail)]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }
            var result = await _userService.UserEmailConfirmation(token, email);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : BadRequest(result);

        }



        /// <summary>
        /// login a registered user using only username and password
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>An access and refresh token for user without 2FA and OTP for user with 2FA</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TokenDto), 200)]
        [ProducesResponseType(400)]

        [Route(AuthenticationRoutes.LoginUser)]

        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }

            var result = await _userService.LoginUserAsync(loginDto);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// login a registered user using OTP code
        /// </summary>
        /// <param name="twoFactorModel"></param>
        /// <returns>An access and refresh token</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TokenDto), 200)]
        [ProducesResponseType(400)]
        //[ValidateAntiForgeryToken]
        [Route(AuthenticationRoutes.LoginUserWith2FA)]
        public async Task<IActionResult> Login2FA(TwoFactorModel twoFactorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }
            var result = await _userService.LoginTwoFactorUserAsync(twoFactorModel);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// allows a user to make a request to reset their current password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(AuthenticationRoutes.ResetUserPassword)]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(model);
        }

        /// <summary>
        /// allows a user to provide credentials to reset their current password
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(AuthenticationRoutes.ResetUserPassword)]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }
            var result = await _userService.PasswordResetAsync(resetPassword);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Sends a request to reset the user's password based on their email address.
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(AuthenticationRoutes.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }
            var result = await _userService.ForgotPasswordRequest(forgotPassword);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var resetPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token = result.Data, email = forgotPassword.Email }, Request.Scheme);
                var message = new EmailMessage(to: new string[] { forgotPassword.Email }, subject: "Password Reset Confirmation Link", body: resetPasswordLink);
                var responseMessage = _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        /// <summary>
        /// allows a user to get a new access and refresh token 
        /// </summary>
        /// <param name="tokenDto"></param>
        /// <returns>A new access and refresh token</returns>
        /// <response code="200">If a new access and refresh token was generated</response>
        /// <response code="400">If token provided is invalid</response>
        [HttpPost]
        [Route(AuthenticationRoutes.RefreshUserToken)]
        [ProducesResponseType(typeof(TokenDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Refresh(TokenDto tokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(APIResponse<ModelStateDictionary>.Create(HttpStatusCode.BadRequest, ModelState, "Validation Failed"));
            }
            var result = await _userService.RefreshToken(tokenDto);
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : StatusCode((int)result.StatusCode, result);
        }
        [HttpGet]
        [Route(AuthenticationRoutes.LogOutUser)]
        public async Task<IActionResult> LogOut()
        {
            var result = await _userService.LogoutUserAsync();
            return result.StatusCode == HttpStatusCode.OK ? Ok(result) : StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("registered-users")]
        public async Task<IActionResult> GetRegisteredUsers()
        {
            var result = await _userService.GetUsers();
            return Ok(result);
        }
    }
}
