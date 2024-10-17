using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class PasswordRepository : EfRepositoryBase<Password, Guid, BaseDbContext>, IPasswordRepository
{
    public PasswordRepository(BaseDbContext context) : base(context)
    {
    }
}
