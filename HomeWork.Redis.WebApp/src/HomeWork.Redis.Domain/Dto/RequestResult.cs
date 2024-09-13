namespace HomeWork.Redis.Domain.Dto
{
    public class RequestResult
    {
        public string Message { get; set; }

        public Codes Code { get; set; }
    }

    public enum Codes
    {
        Unknown = 0,
        BadRequest = 1,
        Success = 2
    }
}
