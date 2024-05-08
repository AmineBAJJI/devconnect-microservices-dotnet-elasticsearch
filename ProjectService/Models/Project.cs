using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectService.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required] [StringLength(255)] public string Title { get; set; }

    [StringLength(1000)] public string Description { get; set; }

    [Required] [StringLength(50)] public string Progress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required] public int OwnerId { get; set; }

    [Range(1, int.MaxValue)] public int MaxParticipants { get; set; }

    public List<string> Participants { get; set; } = new List<string>();

    public List<Comment> Comments { get; set; } = new List<Comment>();
}