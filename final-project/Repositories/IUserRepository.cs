using final_project.Models;

namespace final_project.Repositories;

public interface IUserRepository: IBaseRepository<User>
{
    public bool Exists(string email);
    
    public User? FindByEmail(string email);
}