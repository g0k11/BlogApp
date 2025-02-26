namespace BlogApp.AuthenticationApi.DTOs
{
    public class ChangePasswordRequest
    {
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
