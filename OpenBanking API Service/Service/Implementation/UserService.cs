
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests;
using OpenBanking_API_Service.Dtos.AuthenticationDtos.Responses;
using OpenBanking_API_Service.Service.Constants;
using OpenBanking_API_Service.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly ApplicationDbContext _context;

        private ApplicationUser? _user;

        public UserService(UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            SignInManager<ApplicationUser> signInManager,
                            IEmailService emailService,
                            ILogger<UserService> logger,
                            IConfiguration configuration,
                            ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }


        public async Task<APIResponse<object>> LogoutUserAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return APIResponse<object>.Create(HttpStatusCode.OK, "Successful", null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return APIResponse<object>.Create(HttpStatusCode.InternalServerError, null, "Something went wrong, try again later");
            }
        }

        public async Task<APIResponse<object>> RegisterNewUser(RegisterUserDto registerUser)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(registerUser.UserName);
                if (userExists != null)
                    return APIResponse<object>.Create(HttpStatusCode.BadRequest, $"Username '{registerUser.UserName}' has already been taken.", null);

                ApplicationUser user = new ApplicationUser
                {
                    Email = registerUser.Email,
                    UserName = registerUser.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = registerUser.EnableTwoFactor,
                    PhoneNumber = registerUser.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, registerUser.Password);

                if (!result.Succeeded)
                {
                    return APIResponse<object>.Create(HttpStatusCode.BadRequest, null, "User registration unsuccessful");
                }

                await _userManager.AddToRoleAsync(user, "User");

                _logger.LogInformation("User with username '{@user}' created successfully and role assigned.", user.UserName);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                _logger.LogInformation("Email confirmation token {@token} generated for new user", token);

                // Send mail with the generated email token
                //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token = tokenResponse.Data, email = registerUserDto.Email }, Request.Scheme);
                //var message = new EmailMessage(to: new string[] { registerUser.Email }, subject: "Email Confirmation Code", body: token);

                //_emailService.SendEmail(message);

                //return APIResponse<object>.Create(HttpStatusCode.OK, "Email confirmation token has been sent to your registered email.", token);
                return APIResponse<object>.Create(HttpStatusCode.OK, token, null);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong with the {nameof(RegisterNewUser)} method: {ex.Message}");
                return APIResponse<object>.Create(HttpStatusCode.InternalServerError, null, "User registration unsuccessful");
            }
        }



        public async Task<APIResponse<object>> UserEmailConfirmation(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return APIResponse<object>.Create(HttpStatusCode.NotFound, null, "User does not exist");

            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return APIResponse<object>.Create(HttpStatusCode.OK, "User email verification successful", null);
            }
            return APIResponse<object>.Create(HttpStatusCode.BadRequest, null, "User email verification failed.");
        }
        public async Task<APIResponse<object>> LoginUserAsync(LoginDto loginDto)
        {
            _user = await _userManager.FindByNameAsync(loginDto.UserName);

            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, loginDto.Password));

            if (!result)
            {
                _logger.LogWarning($"{nameof(LoginUserAsync)}: Authentication Failed. Invalid username or password");
                return APIResponse<object>.Create(HttpStatusCode.BadRequest, null, "Invalid username or password");
            }

            await _signInManager.SignOutAsync();
            var signIn = await _signInManager.PasswordSignInAsync(_user, loginDto.Password, loginDto.RememberMe, false);

            if (signIn.RequiresTwoFactor)
            {
                var otpToken = await _userManager.GenerateTwoFactorTokenAsync(_user, "Email");
                _logger.LogInformation($"{nameof(LoginUserAsync)}::Two factor token '{otpToken} generated for {_user.UserName}.'");


                var message = new EmailMessage(new string[] { _user.Email }, "OTP Confirmation Code", otpToken);
                _emailService.SendEmail(message);
                return APIResponse<object>.Create(HttpStatusCode.OK, otpToken, null);
            }
            if (signIn.IsLockedOut)
            {
                return APIResponse<object>.Create(HttpStatusCode.BadRequest, null, "Account has been locked out.");
            }

            var token = await CreateToken(populateExp: true);
            return APIResponse<object>.Create(HttpStatusCode.OK, new TokenDto(token.Data.AccessToken, token.Data.RefreshToken), null);

        }

        public async Task<APIResponse<TokenDto>> LoginTwoFactorUserAsync(TwoFactorModel twoFactorModel)
        {
            _user = await _userManager.FindByNameAsync(twoFactorModel.UserName);

            if (_user == null)
            {
                return APIResponse<TokenDto>.Create(HttpStatusCode.BadRequest, null, "User does not exist");
            }
            //await _signInManager.SignOutAsync();
            var result = await _signInManager.TwoFactorSignInAsync("Email", twoFactorModel.OTP, twoFactorModel.RememberMe, false);
            if (result.Succeeded)
            {
                return await CreateToken(populateExp: false);
            }
            if (result.IsLockedOut)
            {
                return APIResponse<TokenDto>.Create(HttpStatusCode.BadRequest, null, "Account has been locked out.");
            }
            return new APIResponse<TokenDto>(HttpStatusCode.BadRequest, null, "Login process unsuccessful");
        }


        public async Task<APIResponse<string>> ForgotPasswordRequest(ForgotPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                return APIResponse<string>.Create(HttpStatusCode.OK, token, null);

            }
            return APIResponse<string>.Create(HttpStatusCode.BadRequest, null, "Request unsuccessful");
        }

        public async Task<APIResponse<object>> PasswordResetAsync(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return APIResponse<object>.Create(HttpStatusCode.BadRequest, null, "User does not exist");

            }
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (resetPasswordResult.Succeeded)
            {
                return APIResponse<object>.Create(HttpStatusCode.OK, null, "Password successfully changed.");
            }
            return APIResponse<object>.Create(HttpStatusCode.BadRequest, data: null, "Password change unsuccessful.");
        }

        public async Task<APIResponse<TokenDto>> CreateToken(bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            _user.RefreshToken = refreshToken;
            if (populateExp)
                _user.RefreshTokenExpiryDate = DateTime.Now.AddDays(14);
            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return APIResponse<TokenDto>.Create(HttpStatusCode.OK, new TokenDto(accessToken, refreshToken), null);
        }

        public async Task<APIResponse<TokenDto>> RefreshToken(TokenDto token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.Now)
            {
                return APIResponse<TokenDto>.Create(HttpStatusCode.BadRequest, null, "Invalid request. The request sent has some invalid values.");
            }
            _user = user;
            return await CreateToken(populateExp: false);
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
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out
           securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
           !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        #endregion
        #region JWT methods
        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.NameIdentifier, _user.Id)

            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            _ = int.TryParse(jwtSettings["expires"], out int tokenValidityInDays);
            var tokenOptions = new JwtSecurityToken
                (

                    issuer: jwtSettings["ValidIssuer"],
                    audience: jwtSettings["ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(tokenValidityInDays),
                    signingCredentials: signingCredentials
                );
            return tokenOptions;
        }

        public async Task<object> GetUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }
        #endregion
    }
}
