using File_Store_App.Entities;

namespace File_Store_App.Models
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}