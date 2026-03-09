using System.Security.Cryptography;

namespace Core.Security.Hashing;

public static class AES256HashingHelper
{
    /// <summary>
    /// Verilen metni AES-256 algoritması kullanarak şifreler.
    /// </summary>
    /// <param name="plainText">Şifrelenecek metin.</param>
    /// <param name="key">AES anahtarı (32 byte).</param>
    /// <returns>Şifrelenmiş veri (IV + CipherText) byte dizisi olarak döner.</returns>
    public static async Task<byte[]> EncryptBytesAsync(string plainText, byte[] key)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentException("Şifrelenmek istenen metin boş olamaz.", nameof(plainText));

        if (key == null || key.Length != 32)
            throw new ArgumentException("AES anahtarı 32 byte uzunluğunda olmalıdır.", nameof(key));

        // Rastgele bir IV oluştur
        byte[] iv = new byte[16];
        RandomNumberGenerator.Fill(iv);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using StreamWriter writer = new(cryptoStream);

        try
        {
            await writer.WriteAsync(plainText);
            await writer.FlushAsync(); // Yazma işlemini tamamla
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Şifreleme sırasında hata: {ex.Message}");
            throw new CryptographicException("Şifreleme işlemi sırasında bir hata oluştu.", ex);
        }

        writer.Close();

        byte[] encryptedContent = memoryStream.ToArray();
        if (encryptedContent.Length == 0)
            throw new CryptographicException("Şifrelenmiş içerik oluşturulamadı.");

        // IV'yi ve şifrelenmiş içeriği birleştir
        byte[] result = new byte[iv.Length + encryptedContent.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

        return result;
    }

    /// <summary>
    /// Şifrelenmiş veriyi AES-256 algoritması kullanarak çözer.
    /// </summary>
    /// <param name="cipherBytes">Şifrelenmiş veri (IV + CipherText).</param>
    /// <param name="key">AES anahtarı (32 byte).</param>
    /// <returns>Çözülmüş düz metin.</returns>
    public static async Task<string> DecryptBytesAsync(byte[] cipherBytes, byte[] key)
    {
        if (cipherBytes == null || cipherBytes.Length <= 16)
            throw new ArgumentException("Şifrelenmiş veri geçersiz.", nameof(cipherBytes));

        if (key == null || key.Length != 32)
            throw new ArgumentException("AES anahtarı 32 byte uzunluğunda olmalıdır.", nameof(key));

        // IV'yi ve şifrelenmiş içeriği ayrıştır
        byte[] iv = new byte[16];
        byte[] cipher = new byte[cipherBytes.Length - iv.Length];

        Buffer.BlockCopy(cipherBytes, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(cipherBytes, iv.Length, cipher, 0, cipher.Length);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream memoryStream = new(cipher);
        using CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader reader = new(cryptoStream);

        try
        {
            return await reader.ReadToEndAsync(); // Şifrelenmiş veriyi çöz ve döndür
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Şifre çözme sırasında hata: {ex.Message}");
            throw new CryptographicException("Şifre çözme işlemi sırasında bir hata oluştu.", ex);
        }
    }
}


