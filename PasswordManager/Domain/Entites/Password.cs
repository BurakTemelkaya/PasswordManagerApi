using Core.Persistence.Repositories;

namespace Domain.Entites;

public class Password : Entity<Guid>
{
    public string Name { get; set; }
    public string EncryptedPassword { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public Password()
    {
        Name = string.Empty;
        EncryptedPassword = string.Empty;
        UserId = default!;
        User = new();
    }

    public Password(string name, string description, string encryptedPassword, int userId, User user)
    {
        Name = name;
        Description = description;
        EncryptedPassword = encryptedPassword;
        UserId = userId;
        User = user;
    }
}
