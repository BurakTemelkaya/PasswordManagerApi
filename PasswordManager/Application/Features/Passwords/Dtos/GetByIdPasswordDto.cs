namespace Application.Features.Passwords.Dtos;

public class GetByIdPasswordDto
{
	public Guid Id { get; set; }
	public byte[] Name { get; set; }
	public byte[] Password { get; set; }
	public string? Description { get; set; }
	public string? WebSiteUrl { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }

	public GetByIdPasswordDto()
	{
		Id = Guid.NewGuid();
		Name = [];
		Password = [];
		CreatedDate = DateTime.UtcNow;
	}

	public GetByIdPasswordDto(Guid id, byte[] name, byte[] password, DateTime createdDate,
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
