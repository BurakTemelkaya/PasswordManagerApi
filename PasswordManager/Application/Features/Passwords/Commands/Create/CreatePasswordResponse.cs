namespace Application.Features.Passwords.Commands.Create;

public class CreatePasswordResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }

    public CreatePasswordResponse()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
    }

    public CreatePasswordResponse(Guid id, DateTime createdDate)
    {
        Id = id;
        CreatedDate = createdDate;
    }
}
