namespace OpenBanking_API_Service.Dtos
{
    public class LoginResponse
    {
        public TokenType AccessToken { get; set; }
    }


    public class TokenType
    {
        public string Token { get; set; }
        public DateTime TokenExpiryDate { get; set; }
    }

}
