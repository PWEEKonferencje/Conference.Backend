using Application.Conferences.AddConferenceTrack;
using AutoMapper;
using Domain.Entities;

namespace Application.Conferences.MapperProfiles;

public class ConferenceMappingProfile : Profile
{
	public ConferenceMappingProfile()
	{
		CreateMap<AddConferenceTrackCommand, Track>();
	}
}