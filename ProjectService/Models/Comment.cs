using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectService.Models;

public class Comment
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [Required] [StringLength(1000)] public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required] public int Author { get; set; }
}