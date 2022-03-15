
using Cart.Application.Common.Exceptions;
using FluentValidation;
using MediatR;
using ValidationException = Cart.Application.Common.Exceptions.ValidationException;

namespace Cart.Application.Common.Behaviours;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);

        var exceptions = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(x => x != null)
            .ToList();

        if (exceptions.Any())
        {
            var validationFailures = new List<ValidationFailureModel>();

            var groupValidationExceptions = exceptions.GroupBy(x => x.PropertyName).ToList();

            foreach (var groupValidationException in groupValidationExceptions)
            {
                var validationFailure = new ValidationFailureModel
                {
                    Field = groupValidationException.Key,
                    Errors = groupValidationException.Select(x => x.ErrorMessage)
                };

                validationFailures.Add(validationFailure);
            }

            throw new ValidationException(validationFailures);
        }

        return await next();
    }
}