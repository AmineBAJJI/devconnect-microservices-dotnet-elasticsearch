using System.ComponentModel.DataAnnotations;

namespace Shared.Events.Project;

public class CommentCreateDto
{
    [Required]
    [StringLength(1000)]
    public string Content { get; set; }

    [Required]
    public int Author { get; set; }
}