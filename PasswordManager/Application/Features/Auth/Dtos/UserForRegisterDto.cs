using Core.Application.Dtos;

namespace Application.Features.Auth.Dtos;

public class UserForRegisterDto : IDto
{
    public string UserName { get; set; }
    public string Email { get; set; }

    public byte[] Password { get; set; }
    public byte[] KdfSalt { get; set; }
    public int KdfIterations { get; set; }

    public UserForRegisterDto()
    {
        UserName = string.Empty;
        Email = string.Empty;
        Password = [];
        KdfSalt = [];
    }

    public UserForRegisterDto(string userName, string email, byte[] password, byte[] kdfSalt, int kdfIterations)
    {
        UserName = userName;
        Email = email;
        Password = password;
        KdfSalt = kdfSalt;
        KdfIterations = kdfIterations;
    }
}
