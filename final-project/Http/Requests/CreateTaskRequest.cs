using TaskStatus = final_project.Enums.TaskStatus;

namespace final_project.Http.Requests;

public class CreateTaskRequest
{
    public int customer_id { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public TaskStatus status { get; set; }
    public DateTime started_at { get; set; }
    public DateTime finished_at { get; set; }
}