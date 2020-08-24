using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniURL.Application.PersistedURLs.Commands.Post;
using MiniURL.Application.PersistedURLs.Queries.URLsForUser;

namespace MiniURL.API.Controllers
{
    public class URLController : ApiController
    {
        [HttpGet("{userId}")]
        public async Task<ActionResult<URLsForUserVm>> Get(int userId, bool includeDeleted)
        {
            var response = await Mediator.Send(new GetURLsForUserQuery()
            {
                UserId = userId,
                IncludeDeleted = includeDeleted
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreatePersistedURLCommand command)
        {
            var id = await Mediator.Send(command);

            return Ok(id);
        }
    }
}
