namespace Application.Features.Passwords.Dtos;

public class ImportPasswordDto
{
    public byte[] EncryptedName { get; set; }
    public byte[]? EncryptedUserName { get; set; }
    public byte[] EncryptedPassword { get; set; }
    public byte[]? EncryptedDescription { get; set; }
    public byte[]? EncryptedWebSiteUrl { get; set; }
    public byte[] Iv { get; set; }
    public Guid? UserId { get; set; }

    public ImportPasswordDto()
    {
        EncryptedName = [];
        EncryptedPassword = [];
        Iv = [];
    }

    public ImportPasswordDto(byte[] encryptedName, byte[]? encryptedUserName, byte[] encryptedPassword, byte[]? encryptedDescription, byte[]? encryptedWebSiteUrl, byte[] iv, Guid? userId)
    {
        EncryptedName = encryptedName;
        EncryptedUserName = encryptedUserName;
        EncryptedPassword = encryptedPassword;
        EncryptedDescription = encryptedDescription;
        EncryptedWebSiteUrl = encryptedWebSiteUrl;
        Iv = iv;
        UserId = userId;
    }
}
