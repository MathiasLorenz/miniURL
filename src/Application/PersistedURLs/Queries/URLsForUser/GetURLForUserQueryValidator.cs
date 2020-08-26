using FluentValidation;

namespace MiniURL.Application.PersistedURLs.Queries.URLsForUser
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