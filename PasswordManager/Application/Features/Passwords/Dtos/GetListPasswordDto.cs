namespace Application.Features.Passwords.Dtos;

public class GetListPasswordDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public string? WebSiteUrl { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }

	public GetListPasswordDto()
	{
		Id = Guid.NewGuid();
		Name = string.Empty;
		CreatedDate = DateTime.UtcNow;
	}

	public GetListPasswordDto(Guid id, string name, string? description, string? webSiteUrl, DateTime createdDate, DateTime? updatedDate)
	{
		Id = id;
		Name = name;
		Description = description;
		WebSiteUrl = webSiteUrl;
		CreatedDate = createdDate;
		UpdatedDate = updatedDate;
	}
}
