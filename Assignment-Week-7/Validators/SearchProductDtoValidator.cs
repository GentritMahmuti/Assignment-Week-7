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
                    return x.Count() <= 100;
                });


            RuleFor(c => c.Description)
               .NotEmpty()
               .WithMessage("Name cant be empty")
               .NotNull()
               .WithMessage("Name cant be null");
            RuleFor(c => c.Description)
                .Must(x =>
                {
                    return x.Count() <= 100;
                });

            RuleFor(c => c.Price)
                .GreaterThanOrEqualTo(0.01);

        }   
    }
}
