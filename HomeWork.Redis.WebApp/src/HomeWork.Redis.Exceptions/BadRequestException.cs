using HomeWork.Redis.Domain.Dto;

namespace HomeWork.Redis.Exceptions
{
    public class BadRequestException : Exception
    {
        public readonly string Message;
        public readonly Codes Code;
        public BadRequestException(string message, Codes code)
        {
            Message = message;
            Code = code;
        }
    }
}
