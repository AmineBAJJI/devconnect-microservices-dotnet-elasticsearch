namespace SearchService.Models;

public class Profile
{
    public int PublicId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUri { get; set; }
    public string Domaine { get; set; }
    public string SkillsPrincipale { get; set; }
    public int Level { get; set; }
    public string Username { get; set; }
}