using Core.Application.Responses;

namespace Application.Features.Passwords.Commands.Update;

public class UpdatePasswordResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }
    public DateTime CreatedDate { get; set; }

    public UpdatePasswordResponse()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        CreatedDate = DateTime.UtcNow;
    }

    public UpdatePasswordResponse(Guid id, string name, string description, string webSiteUrl, DateTime createdDate)
    {
        Id = id;
        Name = name;
        Description = description;
        WebSiteUrl = webSiteUrl;
        CreatedDate = createdDate;
    }
}
