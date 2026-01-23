namespace Domain.Entities;

public class User : Core.Security.Entities.User<Guid>
{
    public string UserName { get; set; }
    public byte[] MasterPasswordHash { get; set; }
    public byte[] MasterPasswordSalt { get; set; }
    public byte[] KdfSalt { get; set; }
    public int KdfIterations { get; set; }
    public virtual ICollection<Password> Passwords { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
	public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }

	public User()
    {
        UserName = string.Empty;
        Email = string.Empty;
        MasterPasswordHash = [];
        MasterPasswordSalt = [];
        KdfSalt = [];
        Passwords = new HashSet<Password>();
        RefreshTokens = new HashSet<RefreshToken>();
        UserOperationClaims = new HashSet<UserOperationClaim>();
    }

    public User(string userName, byte[] masterPasswordHash, byte[] masterPasswordSalt, byte[] kdfSalt, int kdfIterations, ICollection<Password> passwords, ICollection<RefreshToken> refreshTokens, ICollection<UserOperationClaim> userOperationClaims)
    {
        UserName = userName;
        MasterPasswordHash = masterPasswordHash;
        MasterPasswordSalt = masterPasswordSalt;
        KdfSalt = kdfSalt;
        KdfIterations = kdfIterations;
        Passwords = passwords;
        RefreshTokens = refreshTokens;
        UserOperationClaims = userOperationClaims;
    }
}
