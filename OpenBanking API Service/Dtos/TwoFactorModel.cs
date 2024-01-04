namespace OpenBanking_API_Service.Dtos
{
    public class TwoFactorModel
    {
        public string OTP { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
