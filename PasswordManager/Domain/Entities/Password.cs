using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Password : Entity<Guid>
{
    public byte[] EncryptedName { get; set; }
    public byte[]? EncryptedUserName { get; set; }
    public byte[] EncryptedPassword { get; set; }
    public byte[]? EncryptedDescription { get; set; }
    public byte[]? EncryptedWebSiteUrl { get; set; }
    public byte[] Iv { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; }

    public Password()
    {
        EncryptedName = [];
        EncryptedPassword = [];
        UserId = default!;
        User = null!;
        Iv = [];
    }

    public Password(byte[] encryptedName, byte[]? encryptedUserName, byte[] encryptedPassword, byte[]? encryptedDescription, byte[]? encryptedWebSiteUrl, byte[] ıv, Guid userId, User user)
    {
        EncryptedName = encryptedName;
        EncryptedUserName = encryptedUserName;
        EncryptedPassword = encryptedPassword;
        EncryptedDescription = encryptedDescription;
        EncryptedWebSiteUrl = encryptedWebSiteUrl;
        Iv = ıv;
        UserId = userId;
        User = user;
    }
}
