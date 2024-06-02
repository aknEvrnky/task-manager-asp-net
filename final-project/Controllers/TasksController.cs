using final_project.DTO;
using final_project.Repositories;
using Microsoft.AspNetCore.Mvc;
using final_project.DTO;

namespace final_project.Controllers;

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
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllTasks([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        // Validate the required parameters
        if (start == default || end == default)
        {
            return BadRequest("Start and end dates are required.");
        }
        // Fetch tasks based on the provided start and end dates
        var tasks = _repository.GetTasksByDateRange(start, end);
        
        var events = tasks.Select(Event.From);
        return Ok(events);
    }
    
}