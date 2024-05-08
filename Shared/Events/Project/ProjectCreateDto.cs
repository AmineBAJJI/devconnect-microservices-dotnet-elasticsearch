using System.ComponentModel.DataAnnotations;

namespace Shared.Events.Project;

public class ProjectCreateDto
{
    [Required]
    [StringLength(255)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [Required]
    [StringLength(50)]
    public string Progress { get; set; }

    [Required]
    public int OwnerId { get; set; }

    [Range(1, int.MaxValue)]
    public int MaxParticipants { get; set; }
}
