using FluentValidation;
using Mapster;
using MediatR;
using ScheduleService.Application.Common.Exceptions;

namespace ScheduleService.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(val => val.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(fail => fail != null)
            .ToList();

        if (failures.Count != 0)
        {
            throw new Exceptions.ValidationException(failures.Adapt<List<ValidationError>>());
        }

        return await next();
    }
}
