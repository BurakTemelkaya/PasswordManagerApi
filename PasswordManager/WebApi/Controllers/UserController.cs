using Application.Features.Users.Commands.UpdatePassword;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordCommand updateUserCommand)
        {
            updateUserCommand.UserId = getUserIdFromRequest();
            UpdateUserPasswordResponse result = await Mediator.Send(updateUserCommand);
            return Ok(result);
        }
    }
}
