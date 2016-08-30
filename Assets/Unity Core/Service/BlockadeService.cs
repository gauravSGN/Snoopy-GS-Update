using Registry;

namespace Service
{
    public interface BlockadeService : SharedService
    {
        bool PopupsBlocked { get; }
        bool SceneChangeBlocked { get; }

        void Add(Blockade blockade);
        void Remove(Blockade blockade);
    }
}
