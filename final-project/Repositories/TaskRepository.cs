using final_project.Data;
using Task = final_project.Models.Task;

namespace final_project.Repositories;

using Dapper;

public class TaskRepository: ITaskRepository
{
    private readonly DapperContext _context;
    
    public TaskRepository(DapperContext context)
    {
        _context = context;
    }
    
    public List<Task> All()
    {
        var query = "SELECT * FROM tasks";
        
        using (var connection = _context.CreateConnection())
        {
            return connection.Query<Task>(query).AsList();
        }
    }
    public List<Task> GetTasksByDateRange(DateTime start, DateTime end)
    {
        var query = "SELECT * FROM tasks WHERE started_at >= @start AND finished_at <= @end";
        
        using (var connection = _context.CreateConnection())
        {
            return connection.Query<Task>(query, new {start, end}).AsList();
        }
    }

    public Task? Find(int id)
    {
        var query = "SELECT * FROM tasks WHERE Id = @Id";
        
        using (var connection = _context.CreateConnection())
        {
            return connection.QueryFirstOrDefault<Task>(query, new { Id = id });
        }
    }

    public void Create(Task task)
    {
        var query = "INSERT INTO tasks (customer_id, user_id, title, content, status, priority, started_at, finished_at) " +
                    "VALUES (@customer_id, @user_id, @title, @content, @status, @priority, @started_at, @finished_at)";
        
        using (var connection = _context.CreateConnection())
        {
            connection.Execute(query, task);
        }
    }

    public void Update(Task task)
    {
        var query = "UPDATE tasks SET customer_id = @customer_id, user_id = @user_id, title = @title, content = @content, " +
                    "status = @status, priority = @priority, started_at = @started_at, finished_at = @finished_at WHERE id = @id";

        using (var connection = _context.CreateConnection())
        {
            connection.Execute(query, task);
        }
    }

    public void Delete(int id)
    {
        var query = "DELETE FROM tasks WHERE id = @Id";
        
        using (var connection = _context.CreateConnection())
        {
            connection.Execute(query, new { Id = id });
        }
    }
}