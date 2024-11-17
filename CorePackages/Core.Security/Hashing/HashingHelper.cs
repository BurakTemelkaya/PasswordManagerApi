using System.Security.Cryptography;

namespace Core.Security.Hashing;

public static class HashingHelper
{
	/// <summary>
	/// Create a master password hash and salt via Rfc2898DeriveBytes.
	/// </summary>
	public static void CreateMasterPasswordHash(string password,out byte[] hash, out byte[] salt)
	{
		salt = new byte[16];
		RandomNumberGenerator.Fill(salt);

		using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
		{
			hash = pbkdf2.GetBytes(32);
		}
	}

	public static bool VerifyMasterPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
	{
		using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 10000))
		{
			var computedHash = pbkdf2.GetBytes(32);

			for (int i = 0; i < computedHash.Length; i++)
			{
				if (computedHash[i] != storedHash[i])
				{
					return false;
				}
			}
			return true;
		}
	}

}
