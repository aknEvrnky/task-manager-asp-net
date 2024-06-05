using Task = final_project.Models.Task;

namespace final_project.DTO;

public class Event
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    
    public string backgroundColor { get; set; } = "#378006";
    
    public int customer_id { get; set; }

    public static Event From(Task task)
    {
        return new Event
        {
            id = task.id,
            title = task.title,
            description = task.content,
            start = task.started_at,
            end = task.finished_at,
            customer_id = task.customer_id
        };
    }
}