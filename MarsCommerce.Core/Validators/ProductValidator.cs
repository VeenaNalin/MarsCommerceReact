using FluentValidation;
using MarsCommerce.Core.Models;

namespace MarsCommerce.Core.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("The Name could not be empty")
                .NotNull().WithMessage("The Name could not be null")
                .MaximumLength(100).WithMessage("The Name length should be less than 100");

            RuleFor(x => x.Description).NotEmpty().WithMessage("The Description could not be empty")
                .NotNull().WithMessage("The Description could not be null")
                .MaximumLength(1000).WithMessage("The Description length should be maximum 1000");

            RuleFor(x => x.StockCount).GreaterThan(0).LessThanOrEqualTo(1000).WithMessage("The StockCount should be between 1 to 1000")
                .NotNull().WithMessage("The StockCount could not be null");

            RuleFor(x => x.Price).GreaterThan(0).LessThanOrEqualTo(200000).WithMessage("The Price should be between 1.00 to 200000.00")
                .NotNull().WithMessage("The Price could not be null");

            RuleFor(x => x.StockKeepingUnit).NotEmpty().WithMessage("The StockKeepingUnit could not be empty")
                .NotNull().WithMessage("The StockKeepingUnit not be null");
            
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("The CategoryId should be greaterthan 0")
                .NotNull().WithMessage("The CategoryId could not be null");
        }
        private bool CheckId(int? id)
        {
            return !id.HasValue || id.Value > 0;
        }
    }
}
