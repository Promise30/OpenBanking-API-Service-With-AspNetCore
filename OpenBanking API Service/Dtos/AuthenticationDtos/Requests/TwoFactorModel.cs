namespace OpenBanking_API_Service.Dtos.AuthenticationDtos.Requests
{
    public class TwoFactorModel
    {
        public string OTP { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
