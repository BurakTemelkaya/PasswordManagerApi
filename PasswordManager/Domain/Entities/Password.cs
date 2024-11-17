using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Password : Entity<Guid>
{
    public string Name { get; set; }
    public byte[] EncryptedPassword { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; }

    public Password()
    {
        Name = string.Empty;
        EncryptedPassword = Array.Empty<byte>();
        UserId = default!;
        User = new();
    }

    public Password(string name, string description, byte[] encryptedPassword, Guid userId, User user)
    {
        Name = name;
        Description = description;
        EncryptedPassword = encryptedPassword;
        UserId = userId;
        User = user;
    }
}
