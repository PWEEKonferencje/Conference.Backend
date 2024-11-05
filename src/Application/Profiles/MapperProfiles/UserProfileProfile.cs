using AutoMapper;
using Domain.Entities;
using Domain.Models.Profile;

namespace Application.Profiles.MapperProfiles;

public class UserProfileProfile : Profile
{
    public UserProfileProfile()
    {
        CreateMap<CreateProfileRequest, UserProfile>();
        CreateMap<UserProfile, CreateProfileResponse>();
    }
}