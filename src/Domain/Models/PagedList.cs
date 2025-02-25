namespace Domain.Models;

public class PagedList<T>(List<T> items, int page, int pageSize, int totalCount)
{
	public List<T> Items { get; set; } = items;
	public int Page { get; set; } = page;
	public int PageSize { get; set; } = pageSize;
	public int TotalPages { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);
	public int CurrentCount { get; set; } = items.Count;
	public int TotalCount { get; set; } = totalCount;
	public bool HasNextPage { get; set; } = page * pageSize < totalCount;
	public bool HasPreviousPage { get; set; } = page > 1;
	
	public PagedList<TModel> Map<TModel>(List<TModel> dtos)
	{
		return new PagedList<TModel>(dtos, Page, PageSize, TotalCount);
	}
}