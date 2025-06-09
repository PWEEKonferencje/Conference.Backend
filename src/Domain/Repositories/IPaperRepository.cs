using Domain.Entities;
using Domain.Models;
using Domain.Models.Papers;

namespace Domain.Repositories;

public interface IPaperRepository : IRepository<Paper>
{
	Task<PagedList<PaperInfoModel>> GetAttendeePapers(int attendeeId, int page, int pageSize, CancellationToken cancellationToken);
}