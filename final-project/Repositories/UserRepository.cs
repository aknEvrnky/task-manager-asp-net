using Dapper;
using final_project.Data;
using final_project.Models;

namespace final_project.Repositories;

public class UserRepository: IUserRepository
{
    private readonly DapperContext _context;
    
    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public List<User> All()
    {
        var query = "SELECT * FROM users";
        
        using (var connection = _context.CreateConnection())
        {
            return connection.Query<User>(query).AsList();
        }
    }

    public User? Find(int id)
    {
        var query = "SELECT * FROM users WHERE Id = @Id";
        
        using (var connection = _context.CreateConnection())
        {
            return connection.QueryFirstOrDefault<User>(query, new { Id = id });
        }
    }

    public User Create(User model)
    {
        var query = "INSERT INTO users (name, email, password) VALUES (@name, @email, @password); SELECT CAST(SCOPE_IDENTITY() as int)";
        
        using (var connection = _context.CreateConnection())
        {
            var id = connection.QuerySingle<int>(query, model);
            model.id = id;
            return model;
        }
    }

    public void Update(User model)
    {
        var query = "UPDATE users SET name = @name, email = @email, password = @password WHERE id = @id";
        
        using (var connection = _context.CreateConnection())
        {
            connection.Execute(query, model);
        }
    }

    public void Delete(int id)
    {
        var query = "DELETE FROM users WHERE id = @id";
        
        using (var connection = _context.CreateConnection())
        {
            connection.Execute(query, new { id });
        }
    }

    public bool Exists(string email)
    {
        var query = "SELECT * FROM users WHERE email = @email";
        
        using (var connection = _context.CreateConnection())
        {
            return connection.QueryFirstOrDefault<User>(query, new { email }) != null;
        }
    }

    public User? FindByEmail(string email)
    {
        var query = "SELECT * FROM users WHERE email = @email";
        
        using (var connection = _context.CreateConnection())
        {
            return connection.QueryFirstOrDefault<User>(query, new { email });
        }
    }
}