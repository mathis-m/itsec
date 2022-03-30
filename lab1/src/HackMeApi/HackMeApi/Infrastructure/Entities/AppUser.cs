using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackMeApi.Infrastructure.Entities;

public class AppUser
{
    public int Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [StringLength(50)]
    public string UserName { get; set; }
    public string Password { get; set; }

    public List<Post> Posts { get; } = new ();
}