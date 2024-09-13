using HomeWork.Redis.Business.Abstractions;
using HomeWork.Redis.Business.Abstractions.DeleteSession;
using HomeWork.Redis.Client.Repository;
using HomeWork.Redis.Configuration;
using HomeWork.Redis.Domain.Dto;
using HomeWork.Redis.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HomeWork.Redis.Business.Handlers.DeleteSession
{
    public class DeleteGameSessionRequest : HandlerBase, IDeleteGameSessionRequest<DeleteGameSessionRequestContext, RequestResult>
    {
        private readonly IRedisRepository _redisRepository;
        private readonly ILogger<DeleteGameSessionRequest> _logger;
        private readonly RedisKeys _redisKeys;
        public DeleteGameSessionRequest(IRedisRepository redisRepository, ILogger<DeleteGameSessionRequest> logger, IOptions<RedisKeys> redisKeys)
        {
            _redisRepository = redisRepository;
            _logger = logger;
            _redisKeys = redisKeys?.Value
                ?? throw new ArgumentNullException(nameof(IOptions<RedisKeys>));
        }

        public async Task<RequestResult> HandleAsync(DeleteGameSessionRequestContext context)
        {
            ArgumentNullException.ThrowIfNull(nameof(context));

            var validation = ValidateContext(context);

            if (!string.IsNullOrEmpty(validation))
            {
                throw new BadRequestException(validation, Codes.BadRequest);
            }

            var isSuccess = await _redisRepository.DeleteAsync(string.Format(_redisKeys.GameSessionKey, context.Id));

            return HandleRedisRepositoryResponse(isSuccess, "deleted");
        }


        private string ValidateContext(DeleteGameSessionRequestContext context)
        {
            if (context.Id == default)
            {
                return "Session id is not specified";
            }

            return string.Empty;
        }
    }
}
