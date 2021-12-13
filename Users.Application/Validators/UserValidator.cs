using FluentValidation;
using System;
using Users.Core.Entities;

namespace Users.Application.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty");
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email cannot be empty");
            RuleFor(x => x.Password)
                 .NotEmpty()
                 .WithMessage("Password cannot be empty");
            RuleFor(x => x.CreatedDate)
                .NotEmpty()
                .Must(date => date <= DateTime.Now)
                .WithMessage("Created date must be defined and not greater than current time");
        }
    }
}
