using Domain.Models;
using Domain.Models.Conference;

namespace Application.Conferences.GetAttendeesInfoList;

public record GetAttendeesInfoListResponse(PagedList<AttendeeInfoModel> Attendees);