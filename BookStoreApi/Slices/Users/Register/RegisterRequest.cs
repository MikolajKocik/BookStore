using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Slices.Users.Register
{
    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
