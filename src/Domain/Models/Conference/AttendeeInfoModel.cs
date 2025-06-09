using Domain.Models.Affiliations;

namespace Domain.Models.Conference;

public class AttendeeInfoModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Degree { get; set; }
    public string? Country { get; set; }
    public string? Position { get; set; }
    public AffiliationInfoModel? Affiliation { get; set; }
    public List<string> Roles { get; set; } = new();
    public int PapersCount { get; set; }
    public int ReviewsCount { get; set; }
    public DateTime RegisteredAt { get; set; }
}