namespace Application.Features.Passwords.Commands.Import;

public class ImportPasswordResponse
{
    public int AddedPasswordCount { get; set; }
    public DateTime AdditionDate { get; set; }


    public ImportPasswordResponse()
    {
        AddedPasswordCount = 0;
        AdditionDate = DateTime.UtcNow;
    }

    public ImportPasswordResponse(int addedPasswordCount, DateTime additionDate)
    {
        AddedPasswordCount = addedPasswordCount;
        AdditionDate = additionDate;
    }
}
