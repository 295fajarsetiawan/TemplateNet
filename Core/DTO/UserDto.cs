using FluentValidation;

namespace Core.DTO;


public class LoginDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class TokenResponseDto
{
    public string Token { get; set; } = String.Empty;
    public string ExpiresIn { get; set; } = String.Empty;
}


public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}