using FluentValidation;
using MarsCommerce.Core.Models;

namespace MarsCommerce.Core.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("The FirstName could not be empty")
               .NotNull().WithMessage("The FirstName could not be null")
               .MaximumLength(250).WithMessage("The FirstName length should be less than 250");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("The LastName could not be empty")
               .NotNull().WithMessage("The LastName could not be null")
               .MaximumLength(250).WithMessage("The LastName length should be less than 250");

            RuleFor(x => x.AzureUserId).NotEmpty().WithMessage("The AzureUserId could not be empty")
               .NotNull().WithMessage("The AzureUserId could not be null");

            RuleFor(x => x.AuthToken).NotEmpty().WithMessage("The AuthToken could not be empty")
               .NotNull().WithMessage("The AuthToken could not be null");
        }
    }
}
