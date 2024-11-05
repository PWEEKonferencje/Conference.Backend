namespace Domain.Models.Profile;

public class CreateProfileRequest
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string? University { get; set; } 
    public string? Degree { get; set; }  
}