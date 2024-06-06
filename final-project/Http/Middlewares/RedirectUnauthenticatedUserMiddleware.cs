namespace final_project.Http.Middlewares;

public class RedirectUnauthenticatedUserMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectUnauthenticatedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            // if request wants json response, return 401
            if (context.Request.Headers["Accept"].Contains("application/json"))
            {
                context.Response.StatusCode = 401;
                return;
            }
            context.Response.Redirect("/Auth/Login");
            return;
        }

        await _next(context);
    }
}