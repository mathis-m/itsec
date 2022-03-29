namespace HackMeApi.DTOs;

public class PostResponseDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Content { get; set; }
}