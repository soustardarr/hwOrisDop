using HomeWork.Redis.Domain.Dto;
using HomeWork.Redis.Exceptions;

namespace HomeWork.Redis.Business.Handlers
{
    public class HandlerBase
    {
        protected RequestResult GetResponse(string message, Codes code)
            => new()
            {
                Message = message,
                Code = code
            };


        protected RequestResult HandleRedisRepositoryResponse(bool isSuccess, string subMessage)
        {
            if (isSuccess)
            {
                return GetResponse($"Session sucessfully {subMessage}.", Codes.Success);
            }

            throw new BadRequestException("Unknown error, please contact with support team.", Codes.Unknown);
        }
    }
}
