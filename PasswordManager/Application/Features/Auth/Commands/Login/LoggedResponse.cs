using Core.Application.Responses;
using Core.Security.Enums;
using Core.Security.JWT;

namespace Application.Features.Auth.Commands.Login;

public class LoggedResponse : IResponse
{
	public AccessToken? AccessToken { get; set; }
	public Domain.Entities.RefreshToken? RefreshToken { get; set; }
	public AuthenticatorType? RequiredAuthenticatorType { get; set; }
    public string? EncryptionKey { get; set; }

    public LoggedHttpResponse ToHttpResponse()
	{
		return new() { AccessToken = AccessToken, RequiredAuthenticatorType = RequiredAuthenticatorType, EncryptionKey = EncryptionKey };
	}

	public class LoggedHttpResponse
	{
		public AccessToken? AccessToken { get; set; }
		public AuthenticatorType? RequiredAuthenticatorType { get; set; }
		public string? EncryptionKey { get; set; }
    }
}
