using FluentValidation;

namespace MiniURL.Application.PersistedURLs.Queries.GetURLsForUser
{
    public class GetURLsForUserQueryValidator : AbstractValidator<GetURLsForUserQuery>
    {
        public GetURLsForUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}