using HomeWork.Redis.Business.Abstractions.CreateSession;
using HomeWork.Redis.Client.Repository;
using HomeWork.Redis.Configuration;
using HomeWork.Redis.Domain;
using HomeWork.Redis.Domain.Dto;
using HomeWork.Redis.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace HomeWork.Redis.Business.Handlers.CreateItems
{
    public class CreateGameSessionRequest : HandlerBase, ICreateGameSessionRequest<CreateGameSessionRequestContext, RequestResult>
    {
        private readonly ILogger<CreateGameSessionRequest> _logger;
        private readonly IRedisRepository _redisRepository;
        private readonly RedisKeys _redisKeys;
        public CreateGameSessionRequest(IRedisRepository redisRepository, ILogger<CreateGameSessionRequest> logger, IOptions<RedisKeys> redisKeysConfiguration)
        {
            _redisRepository = redisRepository;
            _logger = logger;
            _redisKeys = redisKeysConfiguration?.Value ?? throw new ArgumentNullException(nameof(IOptions<RedisKeys>));
        }


        public async Task<RequestResult> HandleAsync(CreateGameSessionRequestContext context)
        {
            ArgumentNullException.ThrowIfNull(nameof(context));

            var validation = ValidateContext(context);

            if (!string.IsNullOrEmpty(validation))
            {
                throw new BadRequestException(validation, Codes.BadRequest);
            }

            try
            {
                var isSuccess = await _redisRepository.SetAsync(string.Format(_redisKeys.GameSessionKey, context.Session.Id), context.Session);

                return HandleRedisRepositoryResponse(isSuccess, "created");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error game session creation.");
                throw;
            }
        }

        private static string ValidateContext(CreateGameSessionRequestContext context)
        {
            string message = "{0} is not specified;";

            var builder = new StringBuilder(capacity: 5);

            if (context.Session is null)
            {
                builder.Append(string.Format(nameof(GameSession), message));
            }

            if (context.Session?.Id == default)
            {
                builder.Append(string.Format("Session id", message));
            }

            if (context.Session?.GameCache is null)
            {
                builder.Append(string.Format(nameof(GameCache), message));
            }

            if (context.Session?.GameCache.Player is null)
            {
                builder.Append(string.Format(nameof(Player), message));
            }

            if (context.Session?.GameCache.Game is null)
            {
                builder.Append(string.Format(nameof(Game), message));
            }

            return string.Join(";/n", builder);
        }
    }
}
