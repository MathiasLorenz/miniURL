using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace MiniURL.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var ctx = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(ctx, cancellationToken)));
                var failures = validationResults.SelectMany(x => x.Errors).Where(x => x != null).ToList();

                if (failures.Count != 0)
                {
                    // Here I use the FluentValidation.ValidationException and not the 'custom' one
                    // that was done in the Clean Arch repo. I don't know why they made their own?
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}