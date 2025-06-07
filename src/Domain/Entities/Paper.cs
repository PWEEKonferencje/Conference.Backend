using Domain.Enums;

namespace Domain.Entities;

public class Paper
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Abstract { get; set; } = string.Empty;
	public string Authors { get; set; } = string.Empty;
	public PaperStatus Status { get; set; } = PaperStatus.Created;
	public File? LatestFile => FileRevisions.OrderByDescending(x => x.Timestamp).FirstOrDefault()?.File;
	
	public virtual List<PaperStatusRevision> StatusRevision { get; set; } = [];
	public virtual List<PaperFileRevision> FileRevisions { get; set; } = [];
	public virtual List<Review> Reviews { get; set; } = [];
	public virtual List<Keyword> Keywords { get; set; } = [];
	
	public int? TrackId { get; set; }
	public Track? Track { get; set; }

	public int CreatorId { get; set; }
	public Attendee Creator { get; set; } = null!;

	public int ConferenceId { get; set; }
	public Conference Conference { get; set; } = null!;
	
	public static Paper Create(
		string title,
		string? abstractText, 
		string? authors, 
		List<string>? keywords,
		Attendee creator,
		Conference conference)
	{
		return new Paper
		{
			Title = title,
			Abstract = abstractText,
			Keywords = keywords?.Select(x => new Keyword{Value = x}).ToList() ?? [],
			Authors = authors ?? string.Empty,
			Status = PaperStatus.Created,
			Creator = creator,
			CreatorId = creator.Id,
			Conference = conference,
			ConferenceId = conference.Id
		};
	}
}