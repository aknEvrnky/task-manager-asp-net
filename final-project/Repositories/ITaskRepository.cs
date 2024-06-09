namespace final_project.Repositories;

using final_project.Models;

public interface ITaskRepository: IBaseRepository<Task>
{
    public List<Task> AllByUser(int userId);
    List<Task> GetTasksByDateRange(int userId, DateTime start, DateTime end);
}