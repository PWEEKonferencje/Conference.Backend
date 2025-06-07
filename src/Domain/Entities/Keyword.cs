namespace Domain.Entities;

public class Keyword
{
	public int Id { get; set; }
	public string Value { get; set; } = string.Empty;

	public int PaperId { get; set; }
	public Paper Paper { get; set; } = null!;
}