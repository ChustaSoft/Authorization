namespace ChustaSoft.Tools.Authorization.Models
{
    public class ResetPasswordCredentials
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}