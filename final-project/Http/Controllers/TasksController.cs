using final_project.DTO;
using final_project.Http.Requests;
using final_project.Repositories;
using Microsoft.AspNetCore.Mvc;
using Task = final_project.Models.Task;

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
    
    [HttpPost]
    public IActionResult Create([FromForm] Task task)
    {
        if (ModelState.IsValid)
        {
            _repository.Create(task);
            return Ok(task);
        }

        return BadRequest();

    }

    [HttpPut("{id}")]
    public IActionResult Edit(int id, [FromForm] Task task)
    {
        var existingTask = _repository.Find(id);
        if (existingTask == null)
        {
            return NotFound("Task not found");
        }
        
        existingTask.title = task.title;
        existingTask.content = task.content;
        existingTask.started_at = task.started_at;
        existingTask.finished_at = task.finished_at;
        existingTask.customer_id = task.customer_id;

        if (ModelState.IsValid)
        {
            _repository.Update(existingTask);

            return Ok();
        }

        return NoContent();
    }
    
    // create a patch route to update task's started_at and finished_at
    // only started_at and finished_at will be passed in the request body
    [HttpPatch("{id}/dates")]
    public IActionResult UpdateDates(int id, [FromForm] TaskUpdateDateRequest taskUpdateDates)
    {
        var existingTask = _repository.Find(id);
        if (existingTask == null)
        {
            return NotFound("Task not found");
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