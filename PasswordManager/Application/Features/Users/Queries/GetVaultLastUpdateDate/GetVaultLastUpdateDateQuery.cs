using Application.Features.Users.Rules;
using Application.Services.Users;
using Core.Application.Pipelines.Authorization;
using Core.Security.Constants;
using MediatR;

namespace Application.Features.Users.Queries.GetVaultLastUpdateDate;

public class GetVaultLastUpdateDateQuery : IRequest<DateTime>, ISecuredRequest
{
    public Guid UserId { get; set; }

    public string[] Roles => [GeneralOperationClaims.User];

    public class GetVaultLastUpdateDateQueryHandler : IRequestHandler<GetVaultLastUpdateDateQuery, DateTime>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserBusinessRules _userBusinessRules;

        public GetVaultLastUpdateDateQueryHandler(IUserRepository userRepository, UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<DateTime> Handle(GetVaultLastUpdateDateQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.User? user = await _userRepository.GetAsync(u => u.Id == request.UserId);

            await _userBusinessRules.UserShouldBeExistsWhenSelected(user);

            return user!.VaultLastUpdatedDate;
        }
    }
}