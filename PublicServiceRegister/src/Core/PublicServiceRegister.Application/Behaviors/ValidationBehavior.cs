using FluentValidation;
using MediatR;
using PublicServiceRegister.Application.Common;

namespace PublicServiceRegister.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = failures.Select(f => f.ErrorMessage).ToList();

            // Create failure result using reflection to handle both Result and Result<T>
            var resultType = typeof(TResponse);
            if (resultType.IsGenericType)
            {
                var method = typeof(Result).GetMethod(nameof(Result.Failure), new[] { typeof(IEnumerable<string>) });
                var genericMethod = method!.MakeGenericMethod(resultType.GetGenericArguments()[0]);
                return (TResponse)genericMethod.Invoke(null, new object[] { errors })!;
            }

            return (TResponse)(object)Result.Failure(errors);
        }

        return await next();
    }
}
