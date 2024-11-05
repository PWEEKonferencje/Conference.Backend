namespace Domain.Models.Profile;

public class CreateProfileResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string? University { get; set; } 
    public string? Degree { get; set; }  
}