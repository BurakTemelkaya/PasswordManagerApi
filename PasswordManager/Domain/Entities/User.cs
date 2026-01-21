namespace Domain.Entities;

public class User : Core.Security.Entities.User<Guid>
{
    public string UserName { get; set; }
    public byte[] MasterPasswordHash { get; set; }
    public byte[] MasterPasswordSalt { get; set; }
    public virtual ICollection<Password> Passwords { get; set; } = default!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
	public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = default!;

	public User()
    {
        UserName = string.Empty;
        Email = string.Empty;
        MasterPasswordHash = [];
        MasterPasswordSalt = [];
    }

    public User(string userName, string email, byte[] masterPasswordHash, byte[] masterPasswordSalt, ICollection<Password> passwords)
    {
        UserName = userName;
        Email = email;
        MasterPasswordHash = masterPasswordHash;
        MasterPasswordSalt = masterPasswordSalt;
        Passwords = passwords;
    }
}
