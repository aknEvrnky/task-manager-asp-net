using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskStatus = final_project.Enums.TaskStatus;

namespace final_project.Http.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICustomerRepository _repository;

    public HomeController(ICustomerRepository repository, ILogger<HomeController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var customers = _repository.All();
        ViewData["customers"] = customers;
        ViewBag.StatusOptions = GetStatusOptions();
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    private List<SelectListItem> GetStatusOptions()
    {
        var statusOptions = Enum.GetValues(typeof(TaskStatus))
            .Cast<TaskStatus>()
            .Select(status => new SelectListItem
            {
                Value = status.GetHashCode().ToString(),
                Text = status.ToString()
            })
            .ToList();

        return statusOptions;
    }
}