namespace Domain.Models.Conference;

public class AddressModel
{
	public string? PlaceName { get; set; }
	public string AddressLine1 { get; set; } = default!;
	public string? AddressLine2 { get; set; }
	public string City { get; set; } = default!;
	public string ZipCode { get; set; } = default!;
	public string Country { get; set; } = default!;
}