using State;

namespace Service
{
    public interface UserStateService : SharedService
    {
        long currentLevel { get; set; }
        long maxLevel { get; set; }
        long hasPaid { get; set; }
        Purchasables purchasables { get; }
        Levels levels { get; }
    }
}
