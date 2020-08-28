using System.Collections.Generic;

namespace MiniURL.Application.PersistedURLs.Queries.GetURLsForUser
{
    public class URLsForUserVm
    {
        public int UserId { get; set; }
        public List<URLsForUserDto> URLs { get; set; }
    }
}