using HomeWork.Redis.Domain.Dto;

namespace HomeWork.Redis.Business.Abstractions
{
    public interface IRequestBase<TRequestContext, TRequestResult>
    {
        Task<TRequestResult> HandleAsync(TRequestContext context);
    }
}
    