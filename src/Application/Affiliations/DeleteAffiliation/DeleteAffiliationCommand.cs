using System.Net;
using Application.Common.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using MediatR;


namespace Application.Affiliations.DeleteAffiliation;

public record DeleteAffiliationCommand(Guid Id) : IRequest<ICommandResult<DeleteAffiliationResponse>>;

internal class DeleteAffiliationCommandHandler(IAuthenticationService authenticationService,IAffiliationRepository affiliationRepository,IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteAffiliationCommand, ICommandResult<DeleteAffiliationResponse>>
{
    public async Task<ICommandResult<DeleteAffiliationResponse>> Handle(DeleteAffiliationCommand request, CancellationToken cancellationToken)
    {
        var user = await authenticationService.GetCurrentUser();

        var affiliation = await affiliationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (affiliation is null)
        {
            return CommandResult.Failure<DeleteAffiliationResponse>(new ErrorResult
            {
                ErrorCode = "Affiliation not found",
                StatusCode = HttpStatusCode.NotFound,
            });
        }

        if (affiliation.UserId != user.Id)
        {
            return CommandResult.Failure<DeleteAffiliationResponse>(new ErrorResult
            {
                ErrorCode = "Forbidden",
                StatusCode = HttpStatusCode.Forbidden,
            });
        }

        affiliationRepository.Delete(affiliation , cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return CommandResult.Success(new DeleteAffiliationResponse());
    }
}
