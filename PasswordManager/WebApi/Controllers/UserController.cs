using Application.Features.Users.Commands.UpdatePassword;
using Application.Features.Users.Queries.GetKdfParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
{
    [Authorize]
    [HttpPut("UpdatePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordCommand updateUserCommand)
    {
        updateUserCommand.UserId = getUserIdFromRequest();
        updateUserCommand.UpdatedPasswords.Select(up => up.UserId = updateUserCommand.UserId.Value);
        UpdateUserPasswordResponse result = await Mediator.Send(updateUserCommand);
        return Ok(result);
    }

    [HttpGet("GetUserKdfParams")]
    public async Task<IActionResult> GetUserKdfParams([FromQuery] GetKdfParamsQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
