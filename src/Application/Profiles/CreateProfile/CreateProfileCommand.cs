using System.Net;
using Application.Affiliations.CreateAffiliation;
using Application.Common.Consts;
using Application.Common.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Profiles.CreateProfile;

public record CreateProfileCommand : IRequest<ICommandResult<CreateProfileResponse>>
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Degree { get; set; } = default!;
    public List<CreateAffiliationModel>? Affiliations { get; set; }
}

internal class CreateProfileCommandHandler (IAuthenticationService authenticationService, IUserContextService userContextService,
    IMapper mapper, IUnitOfWork unitOfWork, IProfileRepository profileRepository)
    : IRequestHandler<CreateProfileCommand, ICommandResult<CreateProfileResponse>>
{
    public async Task<ICommandResult<CreateProfileResponse>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await authenticationService.GetCurrentUserAccount();
        if (userAccount is null)
        {
            return CommandResult.Failure<CreateProfileResponse>(new ErrorResult
            {
                ErrorCode = ValidationError.AuthorizationFailed,
                StatusCode = HttpStatusCode.Unauthorized,
            });
        }

        var profile = await profileRepository
            .GetFirstAsync(x => x.Id == userAccount.UserProfileId, cancellationToken);
        if (profile is not null && profile.IsProfileSetUp)
        {
            return CommandResult.Failure<CreateProfileResponse>(new ErrorResult
            {
                ErrorCode = "ProfileAlreadyExists",
                StatusCode = HttpStatusCode.BadRequest,
            });
        }
        
        var userProfile = mapper.Map<User>(request);
        if (userAccount.UserProfile is not null)
        {
            userProfile.Id = userAccount.UserProfile.Id;
            userProfile.OrcidId = userAccount.UserProfile.OrcidId;
        }
        userAccount.UserProfile = userProfile;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return CommandResult.Success(new CreateProfileResponse());
    }
}
