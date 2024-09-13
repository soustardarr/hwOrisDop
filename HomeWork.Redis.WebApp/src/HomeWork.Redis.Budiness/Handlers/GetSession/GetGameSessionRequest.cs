using HomeWork.Redis.Business.Abstractions.DeleteSession;
using HomeWork.Redis.Business.Abstractions;
using HomeWork.Redis.Business.Abstractions.GetSession;
using HomeWork.Redis.Domain.Dto;
using HomeWork.Redis.Business.Handlers.DeleteSession;
using HomeWork.Redis.Client.Repository;
using HomeWork.Redis.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HomeWork.Redis.Domain;
using HomeWork.Redis.Exceptions;

namespace HomeWork.Redis.Business.Handlers.GetSession
{
    public class GetGameSessionRequest : HandlerBase, IGetSessionRequest<GetGameSessionRequestContext, GameSession?>
    {
        private readonly IRedisRepository _redisRepository;
        private readonly ILogger<DeleteGameSessionRequest> _logger;
        private readonly RedisKeys _redisKeys;
        public GetGameSessionRequest(IRedisRepository redisRepository, ILogger<DeleteGameSessionRequest> logger, IOptions<RedisKeys> redisKeys)
        {
            _redisRepository = redisRepository ??  throw new ArgumentNullException(nameof(IRedisRepository));
            _logger = logger;
            _redisKeys = redisKeys?.Value
                ?? throw new ArgumentNullException(nameof(IOptions<RedisKeys>));
        }

        public async Task<GameSession?> HandleAsync(GetGameSessionRequestContext context)
        {
            ArgumentNullException.ThrowIfNull(nameof(context));

            var validation = ValidateContext(context);

            if (!string.IsNullOrEmpty(validation))
            {
                throw new BadRequestException(validation, Codes.BadRequest);
            }

            try
            {
                return await _redisRepository.GetAsync<GameSession>(string.Format(_redisKeys.GameSessionKey, context.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get session.");

                throw;
            }
        }

        private string ValidateContext(GetGameSessionRequestContext context)
        {
            if (context.Id == default)
            {
                return "Session id is not specified";
            }

            return string.Empty;
        }
    }
}
