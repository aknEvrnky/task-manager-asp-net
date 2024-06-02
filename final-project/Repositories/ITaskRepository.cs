namespace final_project.Repositories;

using final_project.Models;

public interface ITaskRepository: IBaseRepository<Task>
{
    List<Task> GetTasksByDateRange(DateTime start, DateTime end);
}