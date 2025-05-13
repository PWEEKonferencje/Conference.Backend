namespace Application.Profiles.GetProfileSetupInfo;

public class GetProfileSetupInfoResponse
{
    public bool IsAccountSetupFinished { get; set; }
    public bool IsEmailProvided { get; set; }
    public bool IsOrcidProvided { get; set; }
}