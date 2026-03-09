namespace Application.Features.Passwords.Constants;

public static class PasswordMessages
{
    public const string SectionName = "Passwords";
    public const string PasswordNotExist = "PasswordNotExist";
    public const string UserDoesNotMatch = "UserDoesNotMatch";
    public const string EncryptionKeyNotFound = "EncryptionKeyNotFound";

    public static string GetEncryptionCacheKey(Guid userId)
    {
        return $"EncryptionKey_{userId}";
    }
}
