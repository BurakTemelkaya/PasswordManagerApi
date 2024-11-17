namespace Application.Features.Passwords.Dtos;

public class GetByIdPasswordDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Password { get; set; }
	public string? Description { get; set; }
	public string? WebSiteUrl { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }

	public GetByIdPasswordDto()
	{
		Id = Guid.NewGuid();
		Name = string.Empty;
		Password = string.Empty;
		CreatedDate = DateTime.UtcNow;
	}

	public GetByIdPasswordDto(Guid id, string name, string password, DateTime createdDate,
		string? description, string? webSiteUrl, DateTime? updatedDate)
	{
		Id = id;
		Name = name;
		Password = password;
		Description = description;
		WebSiteUrl = webSiteUrl;
		CreatedDate = createdDate;
		UpdatedDate = updatedDate;
	}
}
