using FluentValidation;
using System.Text.RegularExpressions;

namespace BookStoreApi.Slices.Users.Login
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Matches(@"^(?=.*[A-Z])(?=.*\d).{8,}$")
                .WithMessage("Password must be at least 8 characters, with 1 uppercase letter and 1 number.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Matches(@"^[a-zA-Z0-9]+$")
                .WithMessage("Username must contain only letters and numbers.");
        }
    }
}
