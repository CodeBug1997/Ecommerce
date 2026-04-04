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

    public class OutOfStockException(string message): Exception(message)
    {

    }

    public class ValidationException(string message) : Exception(message)
    {

    }
}