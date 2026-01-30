using Application.Features.Passwords.Commands.Create;
using Application.Features.Passwords.Commands.Delete;
using Application.Features.Passwords.Commands.Import;
using Application.Features.Passwords.Commands.Update;
using Application.Features.Passwords.Dtos;
using Application.Features.Passwords.Queries.GetAllPassword;
using Application.Features.Passwords.Queries.GetPasswordList;
using Core.Application.Requests;
using Core.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebApi.Controllers;

[Authorize]
[EnableRateLimiting("user-based")]
[Route("api/[controller]")]
[ApiController]
public class PasswordController : BaseController
{
	[HttpPost]
	public async Task<IActionResult> Add([FromBody] CreatePasswordDto createPasswordDto)
	{
		createPasswordDto.UserId = getUserIdFromRequest();

		CreatePasswordCommand createPasswordCommand = new() { CreatePasswordDto = createPasswordDto };
		CreatePasswordResponse result = await Mediator.Send(createPasswordCommand);

		return Created("", result);
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdatedPasswordDto updatedPasswordDto)
	{
		updatedPasswordDto.UserId = getUserIdFromRequest();

		UpdatedPasswordCommand createPasswordCommand = new() { UpdatedPasswordDto = updatedPasswordDto };
		UpdatePasswordResponse result = await Mediator.Send(createPasswordCommand);

		return Ok(result);
	}

	[HttpGet]
	public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
	{
		GetPasswordListQuery getPasswordListQuery = new() { PageRequest = pageRequest, UserId = getUserIdFromRequest() };
		GetListResponse<GetListPasswordDto> result = await Mediator.Send(getPasswordListQuery);
		return Ok(result);
	}

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        GetAllPasswordQuery getPasswordListQuery = new() { UserId = getUserIdFromRequest() };
        List<GetListPasswordDto> result = await Mediator.Send(getPasswordListQuery);
        return Ok(result);
    }

    [HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] Guid id)
	{
		GetPasswordByIdQuery query = new() { Id = id, UserId = getUserIdFromRequest() };

		GetByIdPasswordDto response = await Mediator.Send(query);

		return Ok(response);
	}

	[HttpDelete]
	public async Task<IActionResult> Delete([FromBody] DeletePasswordCommand deletePasswordCommand )
	{
		deletePasswordCommand.UserId = getUserIdFromRequest();

		DeletePasswordResponse result = await Mediator.Send(deletePasswordCommand);

		return Ok(result);
	}

	[HttpPost("Import")]
	public async Task<IActionResult> ImportPasswords([FromBody] ImportPasswordCommand importPasswordCommand)
	{
		importPasswordCommand.UserId = getUserIdFromRequest();

		ImportPasswordResponse result = await Mediator.Send(importPasswordCommand);

		return Ok(result);
	}
}
