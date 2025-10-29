using Core.DTO;
using FluentValidation;

namespace Template.Mapping;

public static class ValidatorMapping
{
    public static void RegisterValidatorMapping(this IServiceCollection services)
    {
        services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
    }
}