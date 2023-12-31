using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Dtos;
using OpenBanking_API_Service.Service.Interface;
using OpenBanking_API_Service_Common.Library.Models;
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
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            SignInManager<ApplicationUser> signInManager,
                            ILogger<UserService> logger,
                            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<APIResponse<object>> CreateUserWithToken(RegisterUserDto registerUser)
        {
            try
            {
                // Check if user already exists
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

                    _logger.LogInformation("User {@result} created successfully and role added", result);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    return APIResponse<object>.Create(HttpStatusCode.OK, "User created successfully. An email confirmation token has been sent to your mail.", token);

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
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = GetToken(authClaims);
            return new APIResponse<LoginResponse>(HttpStatusCode.OK, "User access token generated successfully.", new LoginResponse() { AccessToken = new TokenType() { Token = new JwtSecurityTokenHandler().WriteToken(jwtToken), TokenExpiryDate = jwtToken.ValidTo } });
        }

        public async Task<APIResponse<LoginOtpResponse>> GetOtpByLoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user != null)
            {
                // Optional: Add logging for debugging
                _logger.LogInformation($"User found: {user.UserName}");

                await _signInManager.SignOutAsync();

                await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, true);


                if (user.TwoFactorEnabled)
                {
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    return new APIResponse<LoginOtpResponse>(HttpStatusCode.OK, "An OTP has been sent to your email.", new LoginOtpResponse { Token = token, User = user, IsTwoFactorEnabled = user.TwoFactorEnabled });
                }

                return new APIResponse<LoginOtpResponse>(HttpStatusCode.OK, "2FA is not enabled.", new LoginOtpResponse { Token = string.Empty, User = user, IsTwoFactorEnabled = user.TwoFactorEnabled });


                //// Optional: Add logging for debugging
                //_logger.LogWarning($"Sign in failed for user: {user.UserName}");

                //return new APIResponse<LoginOtpResponse>(HttpStatusCode.Unauthorized, "Invalid credentials.", null);
            }

            return new APIResponse<LoginOtpResponse>(HttpStatusCode.NotFound, "User does not exist.", null);

        }

        public async Task<APIResponse<LoginResponse>> LogInUserWIthOtpAsync(string otp, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", otp, false, false);
            if (signIn.Succeeded)
            {
                if (user != null)
                {
                    return await GetJwtTokenAsync(user);
                }
            }
            return new APIResponse<LoginResponse>(HttpStatusCode.BadRequest, "Login process unsuccessful", null);
        }

        #region PrivateMethods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            var expirationTimeUTC = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);
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
