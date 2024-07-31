using FluentValidation;

namespace UserList.API.Models
{
    public class UserValidator : AbstractValidator<User> 
    {
        public UserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(u => u.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .Length(2, 50).WithMessage("Surname must be between 2 and 50 characters.");

            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(u => !string.IsNullOrEmpty(u.Email));
        }

    }
}
