namespace HomeWork.Redis.Domain
{
    public class GameSession
    {
        public Guid Id { get; set; }
        public GameCache GameCache { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ClosedAt { get; set; }

    }
}
