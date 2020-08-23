using System.Collections.Generic;

namespace MiniURL.Application.PersistedURLs.Queries.URLsForUser
{
    public class URLsForUserVm
    {
        public int UserId { get; set; }
        public List<URLsForUserDto> URLs { get; set; }
    }
}