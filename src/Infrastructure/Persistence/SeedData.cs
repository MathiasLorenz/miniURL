using System.Collections.Generic;
using MiniURL.Domain.Entities;

namespace MiniURL.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static List<User> Users()
        {
            var users = new List<User>()
            {
                new User
                {
                    FirstName = "Første",
                    LastName = "Manden",
                    Email = "sup@sup.com"
                },
                new User
                {
                    FirstName = "Anden",
                    LastName = "Drengen",
                    Email = "what@up.com"
                }
            };

            return users;
        }

        public static List<PersistedURL> PersistedURLs(List<User> users)
        {
            var persistedURLs = new List<PersistedURL>
            {
                new PersistedURL
                {
                    URL = "www.google.com",
                    ShortURL = "abcdefgh",
                },
                new PersistedURL
                {
                    URL = "https://www.dr.dk/drtv/",
                    ShortURL = "objuyhgl",
                    User = users[0]
                },
                new PersistedURL
                {
                    URL = "https://www.google.com/search?q=ef+core+renames+table&oq=ef+core+renames+table&aqs=chrome..69i57j0l7.10922j0j7&sourceid=chrome&ie=UTF-8",
                    ShortURL = "objurhgi",
                    Deleted = true
                },
                new PersistedURL
                {
                    URL = "https://github.com/MathiasLorenz",
                    ShortURL = "iodfetry",
                    Deleted = false,
                    User = users[1]
                },
                new PersistedURL
                {
                    URL = "https://github.com/MathiasLorenz?tab=repositories",
                    ShortURL = "iodjhgry",
                    Deleted = true,
                    User = users[1]
                }
            };

            return persistedURLs;
        }
    }
}