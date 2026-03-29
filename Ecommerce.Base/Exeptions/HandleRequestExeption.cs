namespace Ecommerce.Base.Exeptions
{
    public class BadRequestException(string message) : Exception(message)
    {
    }

    public class NotFoundException(string message) : Exception(message)
    {
    }

    public class ConflictException(string message) : Exception(message)
    {
    }
}
