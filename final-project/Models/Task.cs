using TaskStatus = final_project.Enums.TaskStatus;

namespace final_project.Models;

public class Task
{
    public int id { get; set; }
    public int customer_id { get; set; }
    public int? user_id { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public TaskStatus status { get; set; } = TaskStatus.Pending;
    public DateTime started_at { get; set; }
    public DateTime finished_at { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

    // public Customer Customer { get; set; }
    // public User User { get; set; }
}