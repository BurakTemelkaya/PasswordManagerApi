using Core.Application.Responses;

namespace Application.Features.Passwords.Commands.Delete;

public class DeletePasswordResponse : IResponse
{
	public Guid Id { get; set; }
}
