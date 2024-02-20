using Microsoft.AspNetCore.Authentication;
using Taxes.Infrastructure.Security.Constants;

namespace Taxes.Infrastructure.Security.Settings
{
    public class AuthenticationSettings : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "CustomAuthenticationScheme";
        public string HeaderName { get; set; } = Headers.Role;
    }
}