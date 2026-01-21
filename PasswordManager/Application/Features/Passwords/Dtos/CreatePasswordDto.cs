namespace Application.Features.Passwords.Dtos;

public class CreatePasswordDto
{
    public byte[] EncryptedName { get; set; }
    public byte[]? EncryptedUserName { get; set; }
    public byte[] EncryptedPassword { get; set; }
    public byte[]? EncryptedDescription { get; set; }
    public byte[]? EncryptedWebSiteUrl { get; set; }
    public byte[] Iv { get; set; }
    public Guid? UserId { get; set; }

    public CreatePasswordDto()
    {
        EncryptedName = [];
        EncryptedPassword = [];
        Iv = [];
    }

    public CreatePasswordDto(byte[] encryptedName, byte[]? encryptedUserName, byte[] encryptedPassword, byte[] iv, byte[]? encryptedDescription, byte[]? encryptedWebSiteUrl, Guid? userId)
    {
        EncryptedName = encryptedName;
        EncryptedUserName = encryptedUserName;
        EncryptedPassword = encryptedPassword;
        EncryptedDescription = encryptedDescription;
        EncryptedWebSiteUrl = encryptedWebSiteUrl;
        UserId = userId;
        Iv = iv;
    }
}
