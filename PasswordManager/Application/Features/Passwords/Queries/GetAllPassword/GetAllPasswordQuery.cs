using Application.Features.Passwords.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.CrossCuttingConcerns.Exception.Types;
using Core.Security.Constants;
using MediatR;

namespace Application.Features.Passwords.Queries.GetAllPassword;

public class GetAllPasswordQuery : IRequest<List<GetListPasswordDto>>, ISecuredRequest
{
    public Guid UserId { get; set; }

    public string[] Roles => [GeneralOperationClaims.User];

    public class GetAllPasswordQueryHandler : IRequestHandler<GetAllPasswordQuery, List<GetListPasswordDto>>
    {
        private readonly IPasswordRepository _passwordRepository;
        private readonly IMapper _mapper;

        public GetAllPasswordQueryHandler(IPasswordRepository passwordRepository, IMapper mapper)
        {
            _passwordRepository = passwordRepository;
            _mapper = mapper;
        }

        public async Task<List<GetListPasswordDto>> Handle(GetAllPasswordQuery request, CancellationToken cancellationToken)
        {
            ICollection<Domain.Entities.Password> password = await _passwordRepository
                .GetAllAsync(x => x.UserId == request.UserId,
                cancellationToken: cancellationToken);

            if (password == null)
            {
                throw new BusinessException("Parola bulunamadı.");
            }

            List<GetListPasswordDto> getListPasswordDto = _mapper.Map<List<GetListPasswordDto>>(password);

            return getListPasswordDto;
        }
    }
}
