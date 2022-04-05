using Ganss.XSS;
using HackMeApi.Infrastructure;
using HackMeApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackMeApi.Services
{
    public class PostService
    {
        private readonly HackMeContext _context;
        private readonly ILogger<PostService> _logger;
        private readonly HtmlSanitizer _sanitizer;

        public PostService(HackMeContext context, ILogger<PostService> logger)
        {
            _context = context;
            _logger = logger;
            _sanitizer = new HtmlSanitizer();
        }

        public async Task CreatePostFor(string userName, string content, string createdAt)
        {
            var user = await _context.AppUser.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user is null)
                throw new ArgumentException("Username not known to the system", nameof(userName));

            var sanitizedContent = _sanitizer.Sanitize(content);

            // do some sketchy sql injection stuff
            // A1:2017-Injection
            // fix don't use ExecuteSqlRawAsync instead use ExecuteSqlInterpolated(DatabaseFacade, FormattableString) or use Ef to generate sql
            // fixed by Robert & Mathis
            _ = await _context.Posts.AddAsync(new Post
            {
                Author = user,
                CreatedAt = DateTime.Parse(createdAt, null, System.Globalization.DateTimeStyles.RoundtripKind),
                Content = sanitizedContent
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllPostsWithAuthor()
        {
            return await _context.Posts
                .OrderByDescending(x => x.CreatedAt)
                .Include(x => x.Author)
                .ToListAsync();
        }
    }
}