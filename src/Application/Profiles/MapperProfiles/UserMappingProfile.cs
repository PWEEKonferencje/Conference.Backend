using Application.Affiliations.CreateAffiliation;
using Application.Profiles.CreateProfile;
using Application.Profiles.GetProfileDetails;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles.MapperProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateProfileCommand, User>()
            .ForMember(x => x.Affiliations,
                x =>
                    x.MapFrom(r => r.Affiliations))
            .ForMember(x => x.IsProfileSetUp,
                x => 
                    x.MapFrom(r => true));
        CreateMap<CreateAffiliationModel, Affiliation>();
        CreateMap<User, GetProfileDetailsResponse>();
    }
}