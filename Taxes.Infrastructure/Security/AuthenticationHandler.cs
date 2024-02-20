using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Taxes.Infrastructure.Security.Constants;
using Taxes.Infrastructure.Security.Settings;

namespace Taxes.Infrastructure.Security
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSettings>
    {
        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationSettings> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.HeaderName))
            {
                return Task.FromResult(AuthenticateResult.Fail("Required header is missing"));
            }

            var value = Request.Headers[Options.HeaderName];
            if (IsValidHeader(value))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid header"));
            }

            var ticket = CreateAuthenticationTicket(value!);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private static bool IsValidHeader(string? value)
        {
            return string.IsNullOrEmpty(value) ||
                (!value.Equals(HeaderValues.Admin) && !value.Equals(HeaderValues.User));
        }

        private static bool IsAdmin(string value)
        {
            return value.Equals(HeaderValues.Admin);
        }

        private AuthenticationTicket CreateAuthenticationTicket(string value)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, IsAdmin(value) ? Roles.Admin : Roles.User)
            };
            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        }
    }
}