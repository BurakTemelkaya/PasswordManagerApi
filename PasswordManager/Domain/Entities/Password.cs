using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Password : Entity<Guid>
{
    public byte[] Name { get; set; }
    public byte[] EncryptedPassword { get; set; }
    public byte[]? UserName { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; }

    public Password()
    {
        Name = [];
        EncryptedPassword = [];
        UserId = default!;
        User = null!;
    }

    public Password(byte[] name, byte[] encryptedPassword, byte[]? userName, string description, Guid userId, User user)
    {
        Name = name;
        EncryptedPassword = encryptedPassword;
        UserName = userName;
        Description = description;
        UserId = userId;
        User = user;
    }
}
