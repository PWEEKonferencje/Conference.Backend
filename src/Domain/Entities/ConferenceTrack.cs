namespace Domain.Entities;

public class ConferenceTrack
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    
    public int ConferenceId { get; set; }
    public Conference Conference { get; set; } = default!;
    
}