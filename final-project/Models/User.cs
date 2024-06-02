namespace final_project.Models;

public class User
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}