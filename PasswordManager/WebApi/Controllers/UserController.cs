using Application.Features.Users.Commands.UpdatePassword;
using Application.Features.Users.Dtos;
using Application.Features.Users.Queries.GetKdfParams;
using Application.Features.Users.Queries.GetVaultLastUpdateDate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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

    [EnableRateLimiting("auth-strict")]
    [HttpGet("GetUserKdfParams")]
    public async Task<IActionResult> GetUserKdfParams([FromQuery] GetKdfParamsQuery query)
    {
        GetKdfParamsDto result = await Mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [EnableRateLimiting("vault-sync")]
    [HttpGet("GetVaultLastUpdateDate")]
    public async Task<IActionResult> GetVaultLastUpdateDate()
    {
        GetVaultLastUpdateDateQuery query = new()
        {
            UserId = getUserIdFromRequest()
        };

        DateTime result = await Mediator.Send(query);

        return Ok(result);
    }
}
