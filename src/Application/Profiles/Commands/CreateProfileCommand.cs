using System.Net;
using Application.Common.Consts;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Models.Profile;
using Domain.Repositories;
using Domain.Shared;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Profiles.Commands;

public record CreateProfileCommand(CreateProfileRequest CreateProfileRequest) : IRequest<ICommandResult<CreateProfileResponse>>;

internal class CreateProfileCommandHandler (UserManager<UserAccount> userManager, IUserContextService userContextService,
    IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProfileCommand, ICommandResult<CreateProfileResponse>>
{
    public async Task<ICommandResult<CreateProfileResponse>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await userManager.GetUserAsync(userContextService.User ?? throw new UnauthorizedAccessException());
        if (userAccount is null)
        {
            return CommandResult.Failure<CreateProfileResponse>(new ErrorResult
            {
                ErrorCode = ValidationError.AuthorizationFailed,
                StatusCode = HttpStatusCode.Unauthorized,
            });
        }
        if (userAccount.UserProfileId is not null)
        {
            return CommandResult.Failure<CreateProfileResponse>(new ErrorResult
            {
                ErrorCode = "ProfileAlreadyExists",
                StatusCode = HttpStatusCode.BadRequest,
            });
        }
        var userProfile = mapper.Map<UserProfile>(request.CreateProfileRequest);
        userAccount.UserProfile = userProfile;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var profileResponse = mapper.Map<CreateProfileResponse>(userProfile);
        
        return CommandResult.Success(profileResponse);
    }
}
