using Application.Affiliations.CreateAffiliation;

namespace Application.Profiles.CreateProfile;

public class CreateProfileRequest
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Degree { get; set; } = default!;
    public List<CreateAffiliationModel>? Affiliations { get; set; }
}