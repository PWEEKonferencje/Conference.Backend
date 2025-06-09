namespace Domain.Models.Affiliations;

public class AffiliationInfoModel
{
    public string? Workplace { get; set; } = default!;
    public string? Position { get; set; } = default!;
    public bool? IsAcademic { get; set; }
}