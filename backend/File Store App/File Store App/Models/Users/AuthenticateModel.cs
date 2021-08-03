using System.ComponentModel.DataAnnotations;

namespace File_Store_App.Models.Users
{
    public class AuthenticateModel
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}