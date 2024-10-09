namespace Social_App.API.Features.Users.RevokeToken
{
    public class RevokeTokenResponse
    {

        public bool Success { get; set; }

        public RevokeTokenResponse(bool success)
        {
            Success = success;
        }
    }
}
