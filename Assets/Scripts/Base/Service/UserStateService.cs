using State;

namespace Service
{
    public interface UserStateService : SharedService
    {
        uint currentLevel { get; set; }
        uint maxLevel { get; set; }
        uint hasPaid { get; set; }
        Inventory inventory { get; }
        Levels levels { get; }
    }
}
