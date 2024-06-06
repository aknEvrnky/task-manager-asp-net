namespace final_project.Contracts;

public interface IAuthenticatable
{
    public int getAuthPrimaryKey();
    public string getAuthIdentifierName();
    public string getAuthIdentifier();
}