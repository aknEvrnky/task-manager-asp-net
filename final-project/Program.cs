using final_project.Actions;
using final_project.Data;
using final_project.Http.Middlewares;
using final_project.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticateAction>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseWhen(context => context.Request.Path == "/", homeApp =>
{
    homeApp.UseMiddleware<RedirectUnauthenticatedUserMiddleware>();
});

// Register middleware for TaskController
app.Map("/api/tasks", taskApp =>
{
    taskApp.UseMiddleware<RedirectUnauthenticatedUserMiddleware>();
    taskApp.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
});

// Register middleware for CustomersController
app.Map("/Customers", customersApp =>
{
    customersApp.UseMiddleware<RedirectUnauthenticatedUserMiddleware>();
    customersApp.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();