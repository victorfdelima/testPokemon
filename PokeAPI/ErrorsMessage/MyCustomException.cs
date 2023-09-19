namespace PokeAPI.ErrorsMessage;

public class MyCustomException : Exception
{
    public MyCustomException()
    {
    }

    public MyCustomException(string message)
        : base(message)
    {
    }

    public MyCustomException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
