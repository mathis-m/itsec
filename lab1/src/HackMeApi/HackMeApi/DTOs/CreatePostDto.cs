namespace HackMeApi.DTOs;

public class CreatePostDto
{
    public string Content { get; set; } = null!;
    public string CreatedAt { get; set; }
}