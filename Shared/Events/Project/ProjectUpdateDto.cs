using System.ComponentModel.DataAnnotations;

namespace Shared.Events.Project;

public class ProjectUpdateDto
{
    [StringLength(255)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [StringLength(50)]
    public string Progress { get; set; }

    [Range(1, int.MaxValue)]
    public int MaxParticipants { get; set; }
}