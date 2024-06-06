using System.ComponentModel.DataAnnotations;
using final_project.Data;
using final_project.Repositories;

namespace final_project.Http.Validations;

public class UniqueEmailAttribute: ValidationAttribute
{
    private IUserRepository _repository;

    private void setRepository(ValidationContext validationContext)
    {
        var serviceProvider = validationContext.GetService(typeof(IServiceProvider)) as IServiceProvider;

        if (serviceProvider == null)
        {
            throw new InvalidOperationException("IServiceProvider is null.");
        }

        var dbContext = serviceProvider.GetService<DapperContext>();
        _repository = new UserRepository(dbContext);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        setRepository(validationContext);

        var email = (string)value;

        if (_repository.Exists(email))
        {
            return new ValidationResult("Email is already taken");
        }

        return ValidationResult.Success;
    }
}