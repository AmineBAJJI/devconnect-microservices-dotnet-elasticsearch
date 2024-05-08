namespace ProfileService.DTOs;

public class ProfileDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUri { get; set; }
    public string Field { get; set; }
    public List<string> Skills { get; set; }
    public float Rate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}