using FluentValidation;

namespace MiniURL.Application.PersistedURLs.Commands.Post
{
    public class CreatePersistedURLCommandValidator : AbstractValidator<CreatePersistedURLCommand>
    {
        public CreatePersistedURLCommandValidator()
        {
            RuleFor(x => x.URL)
                .MaximumLength(300)
                .NotEmpty();
        }
    }
}