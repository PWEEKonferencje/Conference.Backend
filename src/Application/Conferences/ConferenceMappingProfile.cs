using Application.Conferences.CreateConference;
using AutoMapper;
using Domain.Entities;

namespace Application.Conferences;

public class ConferenceMappingProfile : Profile
{
	public ConferenceMappingProfile()
	{
		CreateMap<CreateConferenceCommand, Conference>();
	}
}