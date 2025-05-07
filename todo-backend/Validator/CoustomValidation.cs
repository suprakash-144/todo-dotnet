using FluentValidation;
using todo_backend.Dto;

namespace todo_backend.Validator
{
    public class CoustomValidation: AbstractValidator<CreateUserRequest>
    {
        public CoustomValidation() { 
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Must be a valid email"); ;
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}
