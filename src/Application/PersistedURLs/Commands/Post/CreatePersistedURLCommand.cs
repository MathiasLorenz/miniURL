using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Domain.Entities;

namespace MiniURL.Application.PersistedURLs.Commands.Post
{
    public class CreatePersistedURLCommand : IRequest<int>
    {
        public int? UserId { get; set; } = null;
        public string URL { get; set; }
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
            var user = request?.UserId != null ? await _ctx.Users.FindAsync(request.UserId) : null;
            var shortURL = await GenerateShortURL();

            var persistedURL = new PersistedURL
            {
                URL = request.URL,
                ShortURL = shortURL,
                User = user ??= null
            };

            await _ctx.PersistedURLs.AddAsync(persistedURL);
            await _ctx.SaveChangesAsync(cancellationToken);

            return persistedURL.Id;
        }

        private async Task<string> GenerateShortURL()
        {
            var token = "";

            do
            {
                token = _tokenGenerator.GetUniqueKey(8);
            } while (await _ctx.PersistedURLs.FirstOrDefaultAsync(x => x.ShortURL == token) != null);

            return token;
        }
    }
}