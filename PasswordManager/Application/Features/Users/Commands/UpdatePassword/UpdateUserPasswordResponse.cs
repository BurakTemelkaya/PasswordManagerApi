using Core.Application.Responses;

namespace Application.Features.Users.Commands.UpdatePassword;

public class UpdateUserPasswordResponse : IResponse
{
    public Guid UserId { get; set; }
    public DateTime UpdateDate { get; set; }

    public UpdateUserPasswordResponse()
    {
        
    }

    public UpdateUserPasswordResponse(Guid userId,DateTime updateDate)
    {
        UserId = userId;
        UpdateDate = updateDate;
    }
}
