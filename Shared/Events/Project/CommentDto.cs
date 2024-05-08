using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Shared.Events.Project;

public class CommentDto
{
    public string Id { get; set; }

    [Required] [StringLength(1000)] public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required] public int Author { get; set; }
}