using System.Text.RegularExpressions;

namespace Application.Features.Passwords.Constants;

public static class PasswordRegex
{
    public static readonly Regex StrongPasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{6,}$", RegexOptions.Compiled);

    public static bool StrongPassword(string value)
    {
        return StrongPasswordRegex.IsMatch(value);
    }
}
