using Application.Affiliations.Models;

namespace Application.Conferences.Models;

public record AttendeeDto(int Id, string Name, string Degree, string Country, string Position, AffiliationDto Affiliation, 
    List<string> Roles, int PapersCount, int ReviewsCount, DateTime CreatedAt
);