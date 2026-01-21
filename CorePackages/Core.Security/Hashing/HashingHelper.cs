using System.Security.Cryptography;

namespace Core.Security.Hashing;

public static class HashingHelper
{
	private const int SaltSize = 16; // 16 byte Salt (128 bit)
	private const int HashSize = 32; // 32 byte Hash (256 bit)
	private const int Iterations = 100_000; // 100,000 Iterasyon

	/// <summary>
	/// Verilen şifreyi hashler ve rastgele bir salt ile birlikte döner.
	/// </summary>
	/// <param name="password">Hashlenecek şifre.</param>
	/// <param name="hash">Üretilen hash.</param>
	/// <param name="salt">Üretilen salt.</param>
	public static void CreateMasterPasswordHash(string password, out byte[] hash, out byte[] salt)
	{
		if (string.IsNullOrEmpty(password))
			throw new ArgumentException("Parola boş olamaz.", nameof(password));

		salt = new byte[SaltSize];
		RandomNumberGenerator.Fill(salt); // Rastgele Salt oluştur

		using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
		hash = pbkdf2.GetBytes(HashSize); // 256-bit Hash
	}

	/// <summary>
	/// Verilen şifre ile saklanan hash ve salt'ı doğrular.
	/// </summary>
	/// <param name="password">Doğrulanacak şifre.</param>
	/// <param name="storedHash">Saklanan hash.</param>
	/// <param name="storedSalt">Saklanan salt.</param>
	/// <returns>Hash doğrulama sonucu.</returns>
	public static bool VerifyMasterPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
	{
		if (string.IsNullOrEmpty(password))
			throw new ArgumentException("Parola boş olamaz.", nameof(password));

		if (storedHash == null || storedHash.Length != HashSize)
			throw new ArgumentException($"Geçersiz hash uzunluğu. Beklenen: {HashSize} byte.", nameof(storedHash));

		if (storedSalt == null || storedSalt.Length != SaltSize)
			throw new ArgumentException($"Geçersiz salt uzunluğu. Beklenen: {SaltSize} byte.", nameof(storedSalt));

		using var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations, HashAlgorithmName.SHA256);
		var computedHash = pbkdf2.GetBytes(HashSize);

		// Hash'leri karşılaştır
		return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
	}

	/// <summary>
	/// Verilen şifre ve salt ile bir şifreleme anahtarı türetir.
	/// </summary>
	/// <param name="masterPassword">Türetilecek anahtarın parolası.</param>
	/// <param name="salt">Türetmede kullanılacak salt.</param>
	/// <returns>256-bit şifreleme anahtarı.</returns>
	public static byte[] DeriveEncryptionKey(string masterPassword, byte[] salt)
	{
		if (string.IsNullOrEmpty(masterPassword))
			throw new ArgumentException("Parola boş olamaz.", nameof(masterPassword));

		if (salt == null || salt.Length != SaltSize)
			throw new ArgumentException($"Geçersiz salt uzunluğu. Beklenen: {SaltSize} byte.", nameof(salt));

		using var pbkdf2 = new Rfc2898DeriveBytes(masterPassword, salt, Iterations, HashAlgorithmName.SHA256);
		return pbkdf2.GetBytes(HashSize); // 256-bit Key
	}
}