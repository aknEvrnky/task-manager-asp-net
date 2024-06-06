using System.Security.Claims;
using final_project.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace final_project.Actions;

public class AuthenticateAction
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public AuthenticateAction(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task HandleAsync(IAuthenticatable authenticatable)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        // Create user claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, authenticatable.getAuthPrimaryKey().ToString()),
            new Claim(ClaimTypes.Name, authenticatable.getAuthIdentifierName()),
            new Claim(ClaimTypes.Email, authenticatable.getAuthIdentifier())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
        };

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );
    }
}