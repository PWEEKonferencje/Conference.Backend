namespace Domain.Models.Papers;

public class PaperInfoModel
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string? Track { get; set; }
    public DateTime SubmitDate { get; set; }
    public List<string> Keywords { get; set; } = new();
}