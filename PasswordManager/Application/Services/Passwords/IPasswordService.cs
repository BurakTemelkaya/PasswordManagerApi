using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Passwords;

public interface IPasswordService
{
	Task<Password?> GetAsync(
		Expression<Func<Password, bool>> predicate,
		Func<IQueryable<Password>, IIncludableQueryable<Password, object>>? include = null,
		bool withDeleted = false,
		bool enableTracking = true,
		CancellationToken cancellationToken = default
	);

	Task<IPaginate<Password>?> GetListAsync(
		Expression<Func<Password, bool>>? predicate = null,
		Func<IQueryable<Password>, IOrderedQueryable<Password>>? orderBy = null,
		Func<IQueryable<Password>, IIncludableQueryable<Password, object>>? include = null,
		int index = 0,
		int size = 10,
		bool withDeleted = false,
		bool enableTracking = true,
		CancellationToken cancellationToken = default
	);

	Task<Password> AddAsync(Password operationClaim);
	Task<Password> UpdateAsync(Password operationClaim);
	Task<Password> DeleteAsync(Password operationClaim, bool permanent = false);
}
