namespace Application.Features.Passwords.Dtos;

public class CreatePasswordDto
{
	public string Name { get; set; }
	public string? UserName { get; set; }
	public string Password { get; set; }
	public string? Description { get; set; }
	public string? WebSiteUrl { get; set; }
	public Guid? UserId { get; set; }

	public CreatePasswordDto()
	{
		Name = string.Empty;
		Password = string.Empty;
	}

	public CreatePasswordDto(string name, string userName, string password, string? description, string? webSiteUrl, Guid? userId)
	{
		Name = name;
		UserName = userName;
		Password = password;
		Description = description;
		WebSiteUrl = webSiteUrl;
		UserId = userId;
	}
}
