using Assignment_Week_7.Models.DTOs;
using FluentValidation;
using System.Net;

namespace lifeEcommerce.Validators
{
    public class SerachProductDtoValidator : AbstractValidator<SearchProductDto>
    {
        public SerachProductDtoValidator()
        {
            RuleFor(c => c.ProductName)
                .NotEmpty()
                .WithMessage("Name cant be empty")
                .NotNull()
                .WithMessage("Name cant be null");
            RuleFor(c => c.ProductName)
                .Must(x =>
                {
                    if(x != null)
                    {
                        return x.Length <= 100;
                    }
                    return false;
                });

            RuleFor(c => c.Price)
               .NotEmpty()
               .WithMessage("Price can't be empty")
               .NotNull()
               .WithMessage("Price can't be null")
               .GreaterThanOrEqualTo(0.01);

            RuleFor(c => c.Category)
               .NotEmpty()
               .WithMessage("Category can't be empty")
               .NotNull()
               .WithMessage("Category can't be null");
            RuleFor(c => c.Category)
                .Must(x =>
                {
                    if (x != null)
                    {
                        return x.Length <= 100;
                    }
                    return false;
                });

        }   
    }
}
