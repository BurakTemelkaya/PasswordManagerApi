using Application.Features.Auth.Commands.Register;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
	private readonly WebApiConfiguration _configuration;

	public AuthController(IConfiguration configuration)
	{
		const string configurationSection = "WebAPIConfiguration";
		_configuration =
			configuration.GetSection(configurationSection).Get<WebApiConfiguration>()
			?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
	}

	[HttpPost("Register")]
	public async Task<IActionResult> Register([FromBody] RegisterCommand command)
	{
		RegisterCommand registerCommand = new() { Email = command.Email, Password = command.Password, UserName= command.UserName, IpAddress = getIpAddress() };
		RegisteredResponse result = await Mediator.Send(registerCommand);
		setRefreshTokenToCookie(result.RefreshToken);
		return Created(uri: "", result.AccessToken);
	}

	private string getRefreshTokenFromCookies()
	{
		return Request.Cookies["refreshToken"] ?? throw new ArgumentException("Refresh token is not found in request cookies.");
	}

	private void setRefreshTokenToCookie(RefreshToken refreshToken)
	{
		CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
		Response.Cookies.Append(key: "refreshToken", refreshToken.Token, cookieOptions);
	}

}
