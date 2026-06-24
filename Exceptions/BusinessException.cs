namespace FightClub.Exceptions;

public class BusinessException : AppException
{
    public BusinessException(string message)
        : base(message, (int)HttpStatusCode.Conflict)
    {
    }
}
