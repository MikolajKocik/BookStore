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
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Matches(@"^[a-zA-Z0-9]+$")
                .WithMessage("Username must contain only letters and numbers.");
        }
    }
}
