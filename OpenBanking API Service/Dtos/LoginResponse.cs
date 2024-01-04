namespace OpenBanking_API_Service.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpiryDate { get; set; }
    }

}
