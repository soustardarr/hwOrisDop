using HomeWork.Redis.Business.Abstractions.CreateSession;
using HomeWork.Redis.Business.Abstractions.DeleteSession;
using HomeWork.Redis.Business.Abstractions.GetSession;
using HomeWork.Redis.Business.Handlers.CreateItems;
using HomeWork.Redis.Business.Handlers.DeleteSession;
using HomeWork.Redis.Business.Handlers.GetSession;
using HomeWork.Redis.Client.Repository;
using HomeWork.Redis.Configuration;
using HomeWork.Redis.Domain;
using HomeWork.Redis.Domain.Dto;

namespace HomeWork.Redis.WebApp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureRedis(configuration)
                .AddRequests();

            return services;
        }

        private static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<RedisConfiguration>(configuration.GetSection(nameof(RedisConfiguration)))
                .Configure<RedisKeys>(configuration.GetSection(nameof(RedisKeys)))
                .AddSingleton<IRedisRepository, RedisRepository>();

            return services;
        }

        public static IServiceCollection AddRequests(this IServiceCollection services)
        {
            services.AddScoped<ICreateGameSessionRequest<CreateGameSessionRequestContext, RequestResult>, CreateGameSessionRequest>();
            services.AddScoped<IDeleteGameSessionRequest<DeleteGameSessionRequestContext, RequestResult>, DeleteGameSessionRequest>();
            services.AddScoped<IGetSessionRequest<GetGameSessionRequestContext, GameSession?>, GetGameSessionRequest>();
            return services;
        }
    }
}
