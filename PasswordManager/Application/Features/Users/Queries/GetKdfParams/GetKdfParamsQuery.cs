using Application.Features.Users.Dtos;
using Application.Services.Users;
using AutoMapper;
using Core.Security.Hashing;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Users.Queries.GetKdfParams;

public class GetKdfParamsQuery : IRequest<GetKdfParamsDto>
{
    public string UserName { get; set; }

    public class GetAllPasswordQueryHandler : IRequestHandler<GetKdfParamsQuery, GetKdfParamsDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GetAllPasswordQueryHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<GetKdfParamsDto> Handle(GetKdfParamsQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(u => u.UserName == request.UserName);

            if (user == null)
            {
                byte[] fakeResponse = HashingHelper.GenerateDeterministicSalt(request.UserName, _configuration.GetValue<string>("SaltSecret")!);

                return new GetKdfParamsDto
                { 
                    KdfSalt = fakeResponse,
                    KdfIterations = 600000
                };
            }

            GetKdfParamsDto kdfParamsDto = _mapper.Map<GetKdfParamsDto>(user);

            return kdfParamsDto;
        }
    }
}