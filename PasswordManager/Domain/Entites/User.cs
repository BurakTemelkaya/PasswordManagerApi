using Core.Persistence.Repositories;

namespace Domain.Entites;

public class User : Entity<Guid>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public byte[] MasterPasswordSalt { get; set; }
    public byte[] MasterPasswordHash { get; set; }

    public User()
    {
        UserName = string.Empty;
        Email = string.Empty;
        MasterPasswordHash = Array.Empty<byte>();
        MasterPasswordSalt = Array.Empty<byte>();
    }

    public User(string userName,string email, byte[] masterPasswordSalt, byte[] masterPasswordHash)
    {
        UserName = userName;
        Email = email;
        MasterPasswordSalt = masterPasswordSalt;
        MasterPasswordHash = masterPasswordHash;
    }
}
