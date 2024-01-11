
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests;
using OpenBanking_API_Service.Dtos.AuthenticationDtos.Responses;
using OpenBanking_API_Service.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace OpenBanking_API_Service.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            SignInManager<ApplicationUser> signInManager,
                            IEmailService emailService,
                            ILogger<UserService> logger,
                            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<APIResponse<object>> UserEmailConfirmation(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return APIResponse<object>.Create(HttpStatusCode.NotFound, "User does not exist", null);

            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return APIResponse<object>.Create(HttpStatusCode.OK, "User email verification successful", null);
            }
            return APIResponse<object>.Create(HttpStatusCode.BadRequest, "User email verification failed.", null);
        }

        public async Task<APIResponse<object>> RegisterNewUser(RegisterUserDto registerUser)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(registerUser.Email);
                if (userExists != null)
                {
                    return APIResponse<object>.Create(HttpStatusCode.UnprocessableEntity, "User already exists", null);
                }

                ApplicationUser user = new ApplicationUser
                {
                    Email = registerUser.Email,
                    UserName = registerUser.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = true
                };

                _logger.LogInformation("Creating new user...");

                var result = await _userManager.CreateAsync(user, registerUser.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    _logger.LogInformation("User with username {@user} created successfully and role assigned.", user.UserName);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    _logger.LogInformation("Email confirmation token generated for new user");

                    return APIResponse<object>.Create(HttpStatusCode.OK, "User created successfully. An email confirmation token has been sent to your registered email.", token);

                }

                return APIResponse<object>.Create(HttpStatusCode.BadRequest, "User registration unsuccessful", null);

            }
            catch (Exception)
            {

                return APIResponse<object>.Create(HttpStatusCode.BadRequest, "User registration unsuccessful", null);
            }
        }

        public async Task<APIResponse<LoginResponse>> GetJwtTokenAsync(ApplicationUser user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = GetToken(authClaims);
            return APIResponse<LoginResponse>.Create(HttpStatusCode.OK, "User access token generated successfully.", new LoginResponse { Token = new JwtSecurityTokenHandler().WriteToken(jwtToken), TokenExpiryDate = jwtToken.ValidTo });
        }

        public async Task<APIResponse<LoginOtpResponse>> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return APIResponse<LoginOtpResponse>.Create(HttpStatusCode.BadRequest, "Invalid login credentials", null);
            }
            await _signInManager.SignOutAsync();

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, false);


            if (result.RequiresTwoFactor)
            {

                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                return APIResponse<LoginOtpResponse>.Create(HttpStatusCode.OK, "An OTP code has been generated and sent to your email", new LoginOtpResponse { Token = token });

            }
            if (!result.Succeeded)
            {
                // Handle the case where the password is incorrect
                return APIResponse<LoginOtpResponse>.Create(HttpStatusCode.BadRequest, "Incorrect password", null);
            }

            return APIResponse<LoginOtpResponse>.Create(HttpStatusCode.BadRequest, "Two-factor authentication is not enabled for this user", null);
        }

        public async Task<APIResponse<LoginResponse>> LoginUserWithTwoFactorEnabled(TwoFactorModel twoFactorModel)
        {
            var user = await _userManager.FindByEmailAsync(twoFactorModel.Email);

            if (user == null)
            {
                return APIResponse<LoginResponse>.Create(HttpStatusCode.BadRequest, "User does not exist", null);
            }
            //await _signInManager.SignOutAsync();
            var result = await _signInManager.TwoFactorSignInAsync("Email", twoFactorModel.OTP, twoFactorModel.RememberMe, false);
            if (result.Succeeded)
            {
                return await GetJwtTokenAsync(user);
            }
            if (result.IsLockedOut)
            {
                return APIResponse<LoginResponse>.Create(HttpStatusCode.BadRequest, "Account has been locked out.", null);
            }
            return new APIResponse<LoginResponse>(HttpStatusCode.BadRequest, "Login process unsuccessful", null);
        }
        public async Task<APIResponse<object>> ForgotPasswordRequest(ForgotPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                return APIResponse<object>.Create(HttpStatusCode.OK, "Password reset token generated successfully", token);

            }
            return APIResponse<object>.Create(HttpStatusCode.BadRequest, "Request unsuccessful", null);
        }

        public async Task<APIResponse<object>> PasswordResetAsync(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return APIResponse<object>.Create(HttpStatusCode.BadRequest, "User does not exist", null);

            }
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (resetPasswordResult.Succeeded)
            {
                return APIResponse<object>.Create(HttpStatusCode.OK, "Password successfully changed.", null);
            }
            return APIResponse<object>.Create(HttpStatusCode.BadRequest, "Password change unsuccessful.", null);
        }




        #region PrivateMethods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInDays"], out int tokenValidityInDays);
            var expirationTimeUTC = DateTime.UtcNow.AddDays(tokenValidityInDays);
            var localTimeZone = TimeZoneInfo.Local;
            var expirationTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(expirationTimeUTC, localTimeZone);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expirationTimeInLocalTimeZone,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }


        #endregion
    }
}
