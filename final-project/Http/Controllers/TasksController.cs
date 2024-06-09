using final_project.DTO;
using final_project.Http.Requests;
using final_project.Repositories;
using Microsoft.AspNetCore.Mvc;
using Task = final_project.Models.Task;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace final_project.Http.Controllers;

/*
 * The [Route] attribute specifies how the route to a specific action is constructed. In this case:
 * api: This part of the route is static and indicates that the URL path starts with /api.
 * [controller]: This is a placeholder that gets replaced with the name of the controller.
 * For example, if your controller is named TasksController, [controller] will be replaced with tasks.
 * This means the base URL for this controller will be /api/tasks.
 *
 * The [ApiController] attribute is used to denote that the controller will serve as an API controller,
 * rather than a view-based MVC controller
 */
[Route("api/[controller]")]
[ApiController]
public class TasksController: Controller
{
    private readonly ITaskRepository _repository;
    
    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllTasks([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        // Validate the required parameters
        if (start == default || end == default)
        {
            return BadRequest("Start and end dates are required.");
        }

        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) || userId == 0)
        {
            return Unauthorized();
        }
        
        // Fetch tasks based on the provided start and end dates
        var tasks = _repository.GetTasksByDateRange(userId, start, end);
        
        var events = tasks.Select(Event.From);
        return Ok(events);
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult Create([FromForm] CreateTaskRequest request)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid email or password");
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.Errors = errors;
            
            return BadRequest(request);
        }
        
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) || userId == 0)
        {
            return Unauthorized();
        }

        var task = new Task
        {
            title = request.title,
            content = request.content,
            started_at = request.started_at,
            finished_at = request.finished_at,
            customer_id = request.customer_id,
            status = request.status,
            user_id = userId
        };
        
        _repository.Create(task);

        return Ok(request);
    }

    [Authorize]
    [HttpPut("{id}")]
    public IActionResult Edit(int id, [FromForm] UpdateTaskRequest request)
    {
        var existingTask = _repository.Find(id);
        if (existingTask == null)
        {
            return NotFound("Task not found");
        }

        // authenticate
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) || userId == 0)
        {
            return Unauthorized();
        }

        // authorize
        if (existingTask.user_id != userId)
        {
            return Forbid();
        }
        
        if (ModelState.IsValid)
        {
            existingTask.title = request.title;
            existingTask.content = request.content;
            existingTask.started_at = request.started_at;
            existingTask.finished_at = request.finished_at;
            existingTask.customer_id = request.customer_id;
            existingTask.status = request.status;
            
            _repository.Update(existingTask);

            return Ok();
        }

        ModelState.AddModelError("", "Invalid email or password");
        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        ViewBag.Errors = errors;
        return BadRequest();
    }
    
    // create a patch route to update task's started_at and finished_at
    // only started_at and finished_at will be passed in the request body
    [Authorize]
    [HttpPatch("{id}/dates")]
    public IActionResult UpdateDates(int id, [FromForm] TaskUpdateDateRequest taskUpdateDates)
    {
        var existingTask = _repository.Find(id);
        if (existingTask == null)
        {
            return NotFound("Task not found");
        }
        
        // authenticate
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) || userId == 0)
        {
            return Unauthorized();
        }

        // authorize
        if (existingTask.user_id != userId)
        {
            return Forbid();
        }
        
        existingTask.started_at = taskUpdateDates.started_at ?? existingTask.started_at;
        existingTask.finished_at = taskUpdateDates.finished_at ?? existingTask.finished_at;

        if (ModelState.IsValid)
        {
            _repository.Update(existingTask);

            return Ok();
        }

        return NoContent();
    }
}