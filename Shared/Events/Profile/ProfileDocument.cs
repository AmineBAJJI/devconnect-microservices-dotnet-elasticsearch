namespace Shared.Events.Profile;

public class ProfileDocument
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Bio { get; set; }
    public string Field { get; set; }
    public List<string> Skills { get; set; }
    public float Rate { get; set; }
}