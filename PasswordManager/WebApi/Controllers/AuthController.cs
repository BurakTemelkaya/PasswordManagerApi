using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Auth.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

	[HttpPost("Login")]
	public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
	{
		LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto, IpAddress = getIpAddress() };
		LoggedResponse result = await Mediator.Send(loginCommand);

		if (result.RefreshToken is not null)
			setRefreshTokenToCookie(result.RefreshToken);

		return Ok(result.ToHttpResponse());
	}

	[HttpPost("Register")]
	public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
	{
		RegisterCommand registerCommand = new() { UserForRegisterDto = userForRegisterDto, IpAddress = getIpAddress() };
		RegisteredResponse result = await Mediator.Send(registerCommand);
		setRefreshTokenToCookie(result.RefreshToken);
		return Created(uri: "", result.AccessToken);
	}

	[HttpPut("RevokeToken")]
	public async Task<IActionResult> RevokeToken([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string? refreshToken)
	{
		RevokeTokenCommand revokeTokenCommand =
			new() { Token = refreshToken ?? getRefreshTokenFromCookies(), IpAddress = getIpAddress() };
		RevokedTokenResponse result = await Mediator.Send(revokeTokenCommand);
		return Ok(result);
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
