using HomeWork.Redis.Domain.Enums;

namespace HomeWork.Redis.Domain
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EOperationSystem OperationSystem { get; set; }
    }
}
