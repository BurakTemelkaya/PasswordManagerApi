using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Users;

public interface IUserRepository : IAsyncRepository<User, Guid>
{

}
