using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

public interface IPasswordRepository : IAsyncRepository<Password, Guid>
{

}
