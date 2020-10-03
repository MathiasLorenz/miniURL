using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Exceptions;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Domain.Entities;

namespace MiniURL.Application.PersistedURLs.Commands.Post
{
    public class CreatePersistedURLCommand : IRequest<int>
    {
        public int? UserId { get; set; } = null;
        public string URL { get; set; } = null!;
        public string? DesiredShortURL { get; set; } = null;
    }

    public class CreatePersistedURLCommandHandler : IRequestHandler<CreatePersistedURLCommand, int>
    {
        private readonly IMiniURLDbContext _ctx;
        private readonly ITokenGenerator _tokenGenerator;

        public CreatePersistedURLCommandHandler(IMiniURLDbContext ctx,
                                                ITokenGenerator tokenGenerator)
        {
            _ctx = ctx;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<int> Handle(CreatePersistedURLCommand request,
                                      CancellationToken cancellationToken)
        {
            User? user = null;
            if (request?.UserId != null)
            {
                user = await _ctx.Users.FindAsync(request.UserId);
                if (user == null)
                {
                    throw new BadRequestException("The specified user is unknown.");
                }
            }

            var shortURL = await GetShortURL(request?.DesiredShortURL);

            var persistedURL = new PersistedURL
            {
                URL = request.URL, // request cannot be null, but I still get a warning for that it might be :(
                ShortURL = shortURL,
                User = user
            };

            await _ctx.PersistedURLs.AddAsync(persistedURL);
            await _ctx.SaveChangesAsync(cancellationToken);

            return persistedURL.Id;
        }

        private async Task<string> GetShortURL(string? desiredShortURL)
        {
            if (desiredShortURL != null)
            {
                if (await _ctx.PersistedURLs.FirstOrDefaultAsync(x => x.ShortURL == desiredShortURL) != null)
                {
                    // You could check whether the found short URL matches the original too and just return this in that case...
                    throw new BadRequestException("The requested short URL is already present in the database and cannot be chosen.");
                }
                else
                {
                    return desiredShortURL;
                }
            }
            else
            {
                return await GenerateUniqueShortURL();
            }
        }

        // This should probably be factored out and injected. I can mock the TokenGenerator,
        // which is already done in tests, but I cannot test this by itself.
        private async Task<string> GenerateUniqueShortURL()
        {
            var token = "";

            do
            {
                token = _tokenGenerator.GetUniqueKey();
            } while (await _ctx.PersistedURLs.FirstOrDefaultAsync(x => x.ShortURL == token) != null);

            return token;
        }
    }
}