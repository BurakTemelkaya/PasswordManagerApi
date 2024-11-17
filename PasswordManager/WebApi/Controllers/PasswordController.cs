using Application.Features.Passwords.Commands.Create;
using Application.Features.Passwords.Dtos;
using Application.Features.Passwords.Queries.GetPasswordById;
using Core.Application.Requests;
using Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
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

		[HttpGet]
		public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
		{
			GetPasswordListQuery getPasswordListQuery = new() { PageRequest = pageRequest };
			GetListResponse<GetListPasswordDto> result = await Mediator.Send(getPasswordListQuery);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			GetPasswordByIdQuery query = new() { Id = id, UserId = getUserIdFromRequest() };

			GetByIdPasswordDto response = await Mediator.Send(query);

			return Ok(response);
		}
	}
}
