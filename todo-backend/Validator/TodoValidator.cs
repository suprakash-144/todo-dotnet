using FluentValidation;
using todo_backend.Dto;

namespace todo_backend.Validator
{
    public class TodoValidator : AbstractValidator<CreateTodoRequest>
    {
        public TodoValidator() { 
            RuleFor(x=> x.Title).NotEmpty().WithMessage("Title is Required");
            RuleFor(x=> x.Description).NotEmpty().WithMessage("Description is Required");
        }
    }
}
