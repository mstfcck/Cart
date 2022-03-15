using FluentValidation.Results;

namespace Cart.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<ValidationFailureModel> Errors { get; }
    
    public ValidationException(IEnumerable<ValidationFailure> errors)
    {
        Errors = errors.Select(x => new ValidationFailureModel
        {
            Field = x.PropertyName,
            Errors = new []
            {
                x.ErrorMessage
            }
        });
    }

    public ValidationException(IEnumerable<ValidationFailureModel> errors) : base(null)
    {
        Errors = errors;
    }
    
    public ValidationException(string message, IEnumerable<ValidationFailureModel> errors) : base(message)
    {
        Errors = errors;
    }
}

public class ValidationFailureModel
{
    public ValidationFailureModel()
    {
        Errors = new List<string>();
    }

    public string Field { get; set; }
    public IEnumerable<string> Errors { get; set; }
}
