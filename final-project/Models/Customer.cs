namespace final_project.Models;

public class Customer
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}