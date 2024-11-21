namespace Application.Features.Passwords.Dtos;

public class UpdatedPasswordDto
{
	public Guid? Id { get; set; }
	public string Name { get; set; }
	public string Password { get; set; }
	public string? Description { get; set; }
	public string? WebSiteUrl { get; set; }
	public Guid? UserId { get; set; }

	public UpdatedPasswordDto()
	{
		Name = string.Empty;
		Password = string.Empty;
	}

	public UpdatedPasswordDto(Guid id,string name, string password, string? description, string? webSiteUrl, Guid? userId)
	{
		Id = id;
		Name = name;
		Password = password;
		Description = description;
		WebSiteUrl = webSiteUrl;
		UserId = userId;
	}
}
