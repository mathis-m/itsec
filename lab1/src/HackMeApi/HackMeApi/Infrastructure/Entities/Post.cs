namespace HackMeApi.Infrastructure.Entities;

public class Post
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public int AuthorId { get; set; }
    public AppUser Author { get; set; }
}