using System.ComponentModel.DataAnnotations;

namespace Shared.Events.Project;

public class ProjectDeleteDto
{
    [Required]
    public string Id { get; set; }
}