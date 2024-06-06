using final_project.Contracts;

namespace final_project.Models;

public class User: IAuthenticatable
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

    public int getAuthPrimaryKey()
    {
        return id;
    }

    public string getAuthIdentifierName()
    {
        return name;
    }

    public string getAuthIdentifier()
    {
        return email;
    }
}