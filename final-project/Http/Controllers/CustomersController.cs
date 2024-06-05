using final_project.Models;
using final_project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace final_project.Http.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerRepository _repository;

    public CustomersController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var customers = _repository.All();
        return View(customers);
    }

    public IActionResult Details(int id)
    {
        var customer =  _repository.Find(id);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (ModelState.IsValid)
        {
            _repository.Create(customer);
            return RedirectToAction(nameof(Index));
        }

        return View(customer);
    }

    public IActionResult Edit(int id)
    {
        var customer = _repository.Find(id);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Customer customer)
    {
        if (id != customer.id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _repository.Update(customer);
            return RedirectToAction(nameof(Index));
        }

        return View(customer);
    }

    public IActionResult Delete(int id)
    {
        var customer = _repository.Find(id);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _repository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}