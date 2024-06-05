namespace final_project.Http.Requests;

public class TaskUpdateDateRequest
{
    public DateTime? started_at { get; set; }
    public DateTime? finished_at { get; set; }
}