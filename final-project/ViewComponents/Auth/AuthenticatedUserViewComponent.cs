using Microsoft.AspNetCore.Mvc;

namespace final_project.ViewComponents.Auth;

public class AuthenticatedUserViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedUserViewComponent(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IViewComponentResult Invoke()
    {
        var authenticatedUser = _httpContextAccessor.HttpContext.User;
        return View(authenticatedUser);
    }
}