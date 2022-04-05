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

            var post = await _context.Posts.AddAsync(new Post
            {
                Author = user
            });

            await _context.SaveChangesAsync();

            var entityType = _context.Model.FindEntityType(typeof(Post));
            var tableName = entityType!.GetTableName();

            // do some sketchy sql injection stuff
            // A1:2017-Injection
            // fix don't use ExecuteSqlRawAsync instead use ExecuteSqlInterpolated(DatabaseFacade, FormattableString) or use Ef to generate sql

            var sql = _context.IsSqlLite 
                ? $"UPDATE {tableName} SET Content='{sanitizedContent}', CreatedAt='{createdAt}' WHERE Id={post.Entity.Id};"
                : $"UPDATE public.\"{tableName}\" SET \"Content\"='{sanitizedContent}', \"CreatedAt\"='{createdAt}' WHERE public.\"{tableName}\".\"Id\"={post.Entity.Id};";
            await _context.Database.ExecuteSqlRawAsync(sql);
            await _context.SaveChangesAsync();
            var post1 = _context.Posts.SingleOrDefault(p => p.Id == post.Entity.Id);
            _logger.LogInformation(post1.Content);
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