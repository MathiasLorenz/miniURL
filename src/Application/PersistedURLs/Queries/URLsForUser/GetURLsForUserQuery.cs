using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniURL.Application.PersistedURLs.Queries.URLsForUser
{
    public class GetURLsForUserQuery : IRequest<URLsForUserVm>
    {
        public int UserId { get; set; }
        public bool IncludeDeleted { get; set; } = false;
    }

    public class GetURLsForUserQueryHandler : IRequestHandler<GetURLsForUserQuery, URLsForUserVm>
    {
        private readonly IMiniURLDbContext _ctx;

        public GetURLsForUserQueryHandler(IMiniURLDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<URLsForUserVm> Handle(GetURLsForUserQuery request, CancellationToken cancellationToken)
        {
            // Todo: Throw not found if user can't be found.
            var user = await _ctx.Users.FindAsync(request.UserId);
            var urls = await _ctx.PersistedURLs.Where(x => x.UserId == request.UserId).ToListAsync();

            // This should be possible to do in one go above, no?
            if (request.IncludeDeleted == false)
            {
                urls = urls.Where(x => x.Deleted == false).ToList();
            }

            var urlDtos = urls.Select(x => new URLsForUserDto
            {
                URL = x.URL,
                ShortUrl = x.ShortURL,
                Deleted = x.Deleted
            }).ToList();

            var result = new URLsForUserVm
            {
                UserId = user.Id,
                URLs = urlDtos
            };

            return result;
        }
    }
}
