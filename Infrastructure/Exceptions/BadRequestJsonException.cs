using Microsoft.AspNetCore.Http;

namespace Infrastructure.Exceptions;

public class BadRequestJsonException : BadHttpRequestException
{
    public BadRequestJsonException(string message) : base(message) 
    {
        
    }
}