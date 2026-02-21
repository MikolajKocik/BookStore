using BookStoreApi.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApi.Slices.Users.Login
{
    public static class LoginEndpoint
    {
        public static void MapLoginEndpoint(this WebApplication app)
        {
            app.MapPost("/api/users/login", async (LoginRequest request, AppDbContext context, IValidator<LoginRequest> validator, IConfiguration configuration) =>
            {
                // validation
                var validationResult = validator.Validate(request);
                if(!validationResult.IsValid)
                {
                    return Results.BadRequest(string.Join(", ", "Validation failed", validationResult.Errors));
                }

                // user exist?
                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName);
                if(user is null)
                {
                    return Results.NotFound("User not found");
                }

                // check password
                var hasher = new PasswordHasher<Models.User>();
                var verificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
                var passwordMatches = verificationResult == PasswordVerificationResult.Success
                    || verificationResult == PasswordVerificationResult.SuccessRehashNeeded
                    // fallback for legacy plain-text seeded accounts
                    || user.PasswordHash == request.Password;

                if(!passwordMatches)
                {
                    return Results.Unauthorized();
                }

                // create token JWT
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                // from appsettings
                var jwtSettings = configuration.GetSection("Jwt");
                var secretKey = jwtSettings["Key"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];

                // key / credentials / token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                    );

                // token string
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new { token = tokenString, Token = tokenString });
            });
        }
    }
}
