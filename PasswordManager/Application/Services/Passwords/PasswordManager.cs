using Application.Services.Repositories;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Passwords;

public class PasswordManager : IPasswordService
{
	private readonly IPasswordRepository _passwordRepository;

	public PasswordManager(IPasswordRepository passwordRepository)
	{
		_passwordRepository = passwordRepository;
	}

	public async Task<Password> AddAsync(Password operationClaim)
	{
		Password addedPassword = await _passwordRepository.AddAsync(operationClaim);
		return addedPassword;
	}

	public async Task<Password> DeleteAsync(Password operationClaim, bool permanent = false)
	{
		Password deletedPassword = await _passwordRepository.DeleteAsync(operationClaim);
		return deletedPassword;
	}

	public async Task<Password?> GetAsync(Expression<Func<Password, bool>> predicate, Func<IQueryable<Password>, IIncludableQueryable<Password, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
	{
		Password? password = await _passwordRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
		return password;
	}

	public async Task<IPaginate<Password>?> GetListAsync(Expression<Func<Password, bool>>? predicate = null, Func<IQueryable<Password>, IOrderedQueryable<Password>>? orderBy = null, Func<IQueryable<Password>, IIncludableQueryable<Password, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
	{
		IPaginate<Password> passwordList = await _passwordRepository.GetListAsync(
			predicate,
			orderBy,
			include,
			index,
			size,
			withDeleted,
			enableTracking,
			cancellationToken
		);

		return passwordList;
	}

	public async Task<Password> UpdateAsync(Password operationClaim)
	{
		Password updatedPassword = await _passwordRepository.UpdateAsync(operationClaim);
		return	updatedPassword;
	}
}
