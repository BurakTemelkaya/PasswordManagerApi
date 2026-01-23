namespace Application.Features.Users.Dtos;

public class GetKdfParamsDto
{
    public byte[] KdfSalt { get; set; }
    public int KdfIterations { get; set; }

    public GetKdfParamsDto()
    {
        KdfSalt = [];
    }

    public GetKdfParamsDto(byte[] kdfSalt, int kdfIterations)
    {
        KdfSalt = kdfSalt;
        KdfIterations = kdfIterations;
    }
}