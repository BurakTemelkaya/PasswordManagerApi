namespace Application.Features.Passwords.Dtos;

public class ImportPasswordDto
{
    public string Name { get; set; }
    public string? UserName { get; set; }
    public string Password { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }

    public ImportPasswordDto()
    {
        Name = string.Empty;
        Password = string.Empty;
    }

    public ImportPasswordDto(string name, string userName, string password, string? description, string? webSiteUrl)
    {
        Name = name;
        UserName = userName;
        Password = password;
        Description = description;
        WebSiteUrl = webSiteUrl;
    }
}
