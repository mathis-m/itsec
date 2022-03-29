using System.ComponentModel.DataAnnotations;
using HackMeApi.DTOs;
using HackMeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackMeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly PostService _postService;

        public PostsController(ILogger<PostsController> logger, PostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<PostResponseDto>> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsWithAuthor();

            return posts.Select(p => new PostResponseDto
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                Author = p.Author.UserName,
                CreatedAt = p.CreatedAt,
                Content = p.Content!,
            }).ToList();
        }

        [HttpPost]
        [Authorize]
        public async Task CreatePost([FromBody, Required] CreatePostDto createPost)
        {
            var name = HttpContext.User.Identity?.Name;
            if (name is null)
                throw new InvalidOperationException("Invalid internal State.");

            await _postService.CreatePostFor(name, createPost.Content, createPost.CreatedAt);
        }
    }
}