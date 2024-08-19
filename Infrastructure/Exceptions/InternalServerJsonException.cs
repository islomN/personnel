namespace Infrastructure.Exceptions;

public class InternalServerJsonException : Exception
{
    public InternalServerJsonException(string message) : base(message) 
    {
        
    }
}