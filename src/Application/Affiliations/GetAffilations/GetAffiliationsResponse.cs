using Domain.Models.Affiliations;

namespace Application.Affiliations.GetAffiliations;

public class GetAffiliationsResponse
{
	public List<AffiliationModel> Affiliations { get; set; }
}