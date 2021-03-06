﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Application.Common.Exceptions;
using MiniURL.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniURL.Application.PersistedURLs.Queries.GetURLsForUser
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
            var user = await _ctx.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }

            var urls = await _ctx.PersistedURLs
                .Where(x => x.UserId == request.UserId)
                .AsNoTracking()
                .ToDictionaryAsync(x => x.Id, x => x);

            if (request.IncludeDeleted == false)
            {
                foreach ((var id, var url) in urls)
                {
                    if (url.Deleted)
                    {
                        urls.Remove(id);
                    }
                }
            }

            var urlDtos = urls.Values.Select(x => new URLsForUserDto
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
