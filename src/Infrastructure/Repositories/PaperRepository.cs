using Domain.Entities;
using Domain.Models;
using Domain.Models.Papers;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaperRepository(ConferenceDbContext dbContext) : Repository<Paper>(dbContext), IPaperRepository
{
	public async Task<PagedList<PaperInfoModel>> GetAttendeePapers(int attendeeId, int page, int pageSize, CancellationToken cancellationToken)
	{
		var query = dbContext.Papers
			.Where(p => p.CreatorId == attendeeId)
			.Include(p => p.Track)
			.Include(p => p.Keywords)
			.Include(p => p.FileRevisions)
			.OrderByDescending(p => p.Id);

		var total = await query.CountAsync(cancellationToken);

		var papers = await query
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		var items = papers.Select(p => new PaperInfoModel
		{
			Id = p.Id,
			Title = p.Title,
			Status = p.Status.ToString(),
			Track = p.Track?.Name,
			SubmitDate = p.FileRevisions
				.OrderByDescending(f => f.Timestamp)
				.FirstOrDefault()?.Timestamp ?? DateTime.MinValue,
			Keywords = p.Keywords.Select(k => k.Value).ToList()
		}).ToList();

		return new PagedList<PaperInfoModel>(items, page, pageSize, total);
	}
}