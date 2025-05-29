namespace Application.Conferences.Models;

public record ConferenceDetailsDto(string Name, string? Description, DateTime PaperSubmissionDeadline, DateTime RegistrationDeadline);