using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniURL.Application.Common.Exceptions;
using MiniURL.Application.Common.Interfaces;
using MiniURL.Domain.Entities;

namespace MiniURL.Application.PersistedURLs.Queries.GetPersistedURL
{
    public class GetPersistedURLQuery : IRequest<PersistedURLVm>
    {
        public string ShortURL { get; set; } = null!;
    }

    public class GetPersistedURLQueryHandler : IRequestHandler<GetPersistedURLQuery, PersistedURLVm>
    {
        private readonly IMiniURLDbContext _ctx;
        
        public GetPersistedURLQueryHandler(IMiniURLDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PersistedURLVm> Handle(GetPersistedURLQuery request,
                                                 CancellationToken cancellationToken)
        {
            var persistedUrl = await _ctx.PersistedURLs
                .Where(x => x.Deleted == false)
                .FirstOrDefaultAsync(x => x.ShortURL == request.ShortURL);
            if (persistedUrl == null)
            {
                throw new NotFoundException(nameof(PersistedURL), request.ShortURL);
            }

            var vm = new PersistedURLVm
            {
                ShortURL = persistedUrl.ShortURL,
                URL = persistedUrl.URL
            };

            return vm;
        }
    }
}