namespace Application.Features.Passwords.Dtos;

public class GetByIdPasswordDto
{
    public Guid Id { get; set; }
    public byte[] EncryptedName { get; set; }
    public byte[]? EncryptedUserName { get; set; }
    public byte[] EncryptedPassword { get; set; }
    public byte[]? EncryptedDescription { get; set; }
    public byte[]? EncryptedWebSiteUrl { get; set; }
    public byte[] Iv { get; set; }
    public Guid? UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public GetByIdPasswordDto()
    {
        Id = Guid.NewGuid();
        EncryptedName = [];
        EncryptedPassword = [];
        Iv = [];
        CreatedDate = DateTime.UtcNow;
    }

    public GetByIdPasswordDto(Guid id, byte[] encryptedName, byte[]? encryptedUserName, byte[] encryptedPassword, byte[]? encryptedDescription, byte[]? encryptedWebSiteUrl, byte[] iv, Guid? userId, DateTime createdDate, DateTime? updatedDate)
    {
        Id = id;
        EncryptedName = encryptedName;
        EncryptedUserName = encryptedUserName;
        EncryptedPassword = encryptedPassword;
        EncryptedDescription = encryptedDescription;
        EncryptedWebSiteUrl = encryptedWebSiteUrl;
        Iv = iv;
        UserId = userId;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }
}
