using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MyFirstWebAPI.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // this condition checks if the request has an Authorization header, if not it returns no result
            // the authorization header is set in the controller by adding an attribute: [Authorize]
            if (!Request.Headers.ContainsKey("Authorization")) return Task.FromResult(AuthenticateResult.NoResult());

            var authHeader = Request.Headers["Authorization"].ToString();
            // check if the authorization header starts with "Basic "
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase)) 
                return Task.FromResult(AuthenticateResult.Fail("Unkown Scheme."));

            var encodedCredentials = authHeader["Basic ".Length..];
            var decodedCredentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var usernameAndPassword = decodedCredentials.Split(':');
            if (usernameAndPassword[0] != "admin" || usernameAndPassword[1] != "password") 
                return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password."));

            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, "1"),   
                new Claim(ClaimTypes.Name, usernameAndPassword[0])
            ], "Basic");

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Basic");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
