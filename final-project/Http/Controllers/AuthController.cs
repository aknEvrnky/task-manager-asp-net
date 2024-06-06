using Microsoft.AspNetCore.Mvc;
using final_project.Actions;
using final_project.Http.Requests;
using final_project.Models;
using final_project.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace final_project.Http.Controllers;

public class AuthController: Controller
{
    private IUserRepository _repository;
    private readonly AuthenticateAction _authenticateAction;
    
    public AuthController(IUserRepository repository, AuthenticateAction authenticateAction)
    {
        _repository = repository;
        _authenticateAction = authenticateAction;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.Errors = errors;
            return View(request);
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        // Register user
        var user = new User
        {
            name = request.Name,
            email = request.Email,
            password = hashedPassword
        };
        
        _repository.Create(user);

        await _authenticateAction.HandleAsync(user);

        // Assuming successful registration
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.Errors = errors;
            Console.WriteLine(errors);
            return View(request);
        }
        
        var user = _repository.FindByEmail(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.password))
        {
            ModelState.AddModelError("", "Invalid email or password");
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.Errors = errors;
            return View(request);
        }
        
        await _authenticateAction.HandleAsync(user);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}