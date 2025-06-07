namespace Domain.Entities;

public class File
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Extension { get; set; } = string.Empty;
	public byte[] Content { get; set; } = [];
}