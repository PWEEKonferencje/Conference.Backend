using AutoMapper;
using Domain.Entities;
using Domain.Models.Affiliations;
using Application.Affiliations.GetAffiliations;
namespace Application.Affiliations;

public class AffiliationsMappingProfile : Profile
{
	 public AffiliationsMappingProfile()
    {
        CreateMap<Affiliation, AffiliationModel>();
        CreateMap<List<Affiliation>, GetAffiliationsResponse>()
            .ForMember(dest => dest.Affiliations, opt => opt.MapFrom(src => src));
        CreateMap<AffiliationModel, Affiliation>();
    }
}