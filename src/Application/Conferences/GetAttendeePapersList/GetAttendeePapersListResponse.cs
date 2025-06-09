using Domain.Models;
using Domain.Models.Papers;

namespace Application.Papers.GetAttendeePapersList;

public record GetAttendeePapersListResponse(PagedList<PaperInfoModel> Papers);