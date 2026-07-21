global using System.Net;

namespace FightClub.Application.Exceptions;

/// <summary>
/// Исключение поиска
/// </summary>
/// <param name="message">Сообщение</param>
public class NotFoundException(string message) : AppException(message, (int)HttpStatusCode.NotFound)
{
}
