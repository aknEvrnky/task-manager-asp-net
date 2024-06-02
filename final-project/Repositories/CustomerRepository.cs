using Dapper;
using final_project.Data;
using final_project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Task = System.Threading.Tasks.Task;

namespace final_project.Repositories;

public class CustomerRepository: ICustomerRepository
{
    private readonly DapperContext _context;
    
    public CustomerRepository(DapperContext context)
    {
        _context = context;
    }
    
    public List<Customer> All()
    {
        var query = "SELECT * FROM customers";
        using (var connection = _context.CreateConnection())
        {
            return connection.Query<Customer>(query).AsList();
        }
    }

    public Customer? Find(int id)
    {
        var query = "SELECT * FROM customers WHERE Id = @Id";
        using (var connection = _context.CreateConnection())
        {
            return connection.QueryFirstOrDefault<Customer>(query, new { Id = id });
        }
    }

    public void Create(Customer customer)
    {
        var query = "INSERT INTO customers (name, email, phone) " +
                    "VALUES (@name, @email, @phone)";
        using (var connection = _context.CreateConnection())
        {
            connection.Execute(query, customer);
        }
    }

    public void Update(Customer customer)
    {
        var query = "UPDATE customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone, " +
                    "Address = @Address, City = @City, State = @State, ZipCode = @ZipCode, Country = @Country " +
                    "WHERE Id = @Id";
        using (var connection = _context.CreateConnection())
        {
             connection.Execute(query, customer);
        }
    }

    public void Delete(int id)
    {
        var query = "DELETE FROM customers WHERE Id = @Id";
        using (var connection = _context.CreateConnection())
        {
            connection.Execute(query, new { Id = id });
        }
    }
}