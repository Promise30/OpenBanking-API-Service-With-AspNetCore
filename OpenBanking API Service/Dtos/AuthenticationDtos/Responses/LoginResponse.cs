namespace OpenBanking_API_Service.Dtos.AuthenticationDtos.Responses
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public string RefreshToken { get; set; }
    }

}
