namespace OpenBanking_API_Service.Routes
{
    public static class AuthenticationRoutes
    {
        public const string RegisterUser = "register";
        public const string ConfirmUserEmail = "confirm-email";
        public const string LoginUser = "login";
        public const string LoginUserWith2FA = "login-2FA";
        public const string ResetUserPassword = "reset-password";
        public const string ForgotPassword = "forgot-password";
        public const string RefreshUserToken = "refresh-token";
        public const string LogOutUser = "logout";
    }
}
