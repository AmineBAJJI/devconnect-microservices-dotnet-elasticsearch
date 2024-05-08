using System.ComponentModel.DataAnnotations;

namespace Shared.Events.Project;

public class CommentUpdateDto
{
    [StringLength(1000)]
    public string Content { get; set; }
}