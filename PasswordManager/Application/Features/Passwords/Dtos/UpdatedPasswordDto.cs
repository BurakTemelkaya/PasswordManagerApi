namespace Application.Features.Passwords.Dtos;

public class UpdatedPasswordDto
{
    public Guid Id { get; set; }
    public byte[] EncryptedName { get; set; }
    public byte[]? EncryptedUserName { get; set; }
    public byte[] EncryptedPassword { get; set; }
    public byte[]? EncryptedDescription { get; set; }
    public byte[]? EncryptedWebSiteUrl { get; set; }
    public byte[] Iv { get; set; }
    public Guid? UserId { get; set; }

    public UpdatedPasswordDto()
    {
        EncryptedName = [];
        EncryptedPassword = [];
        Iv = [];
    }

    public UpdatedPasswordDto(Guid ıd, byte[] encryptedName, byte[]? encryptedUserName, byte[] encryptedPassword, byte[]? encryptedDescription, byte[]? encryptedWebSiteUrl, byte[] ıv, Guid? userId)
    {
        Id = ıd;
        EncryptedName = encryptedName;
        EncryptedUserName = encryptedUserName;
        EncryptedPassword = encryptedPassword;
        EncryptedDescription = encryptedDescription;
        EncryptedWebSiteUrl = encryptedWebSiteUrl;
        Iv = ıv;
        UserId = userId;
    }
}
