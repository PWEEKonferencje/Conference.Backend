using Application.Conferences.CreateConference;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Conference;
using Domain.ValueObjects;

namespace Application.Conferences;

public class ConferenceMappingProfile : Profile
{
	public ConferenceMappingProfile()
	{
		CreateMap<CreateConferenceCommand, Conference>()
			.ForMember(x => x.Address, r => r.MapFrom(x => x.Address));
		CreateMap<AddressModel, Address>();
		CreateMap<UserSnapshot, UserSnapshotModel>();
	}
}