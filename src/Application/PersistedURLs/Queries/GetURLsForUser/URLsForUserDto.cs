namespace MiniURL.Application.PersistedURLs.Queries.GetURLsForUser
{
    public class URLsForUserDto
    {
        public string URL { get; set; } = null!;
        public string ShortUrl { get; set; } = null!;
        public bool Deleted { get; set; }
    }
}