using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniURL.Application.PersistedURLs.Commands.Post;
using MiniURL.Application.PersistedURLs.Queries.GetPersistedURL;
using MiniURL.Application.PersistedURLs.Queries.GetURLsForUser;

namespace MiniURL.API.Controllers
{
    public class URLController : ApiController
    {
        // Todo: There might be a compelling argument for moving this into a user controller and be
        // something like /user/{id}/urls/{option:includefalse}.
        // I'm not to strong in this, but I think that would make sense.
        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<URLsForUserVm>> GetForUser(int userId, bool includeDeleted)
        {
            var response = await Mediator.Send(new GetURLsForUserQuery()
            {
                UserId = userId,
                IncludeDeleted = includeDeleted
            });

            return Ok(response);
        }

        [HttpGet("{shortURL}")]
        public async Task<ActionResult<int>> GetOriginalFromShortURL(string shortURL)
        {
            var response = await Mediator.Send(new GetPersistedURLQuery() { ShortURL = shortURL });

            return Redirect(response.URL);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreatePersistedURLCommand command)
        {
            var id = await Mediator.Send(command);

            return Ok(id);
        }
    }
}
