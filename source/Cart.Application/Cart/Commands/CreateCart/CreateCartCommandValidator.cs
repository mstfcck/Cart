using FluentValidation;

namespace Cart.Application.Cart.Commands.CreateCart;

public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0);
    }
}