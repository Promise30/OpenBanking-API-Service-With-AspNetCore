namespace OpenBanking_API_Service.Dtos.AuthenticationDtos.Responses
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public TokenDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;

        }
    }
}
