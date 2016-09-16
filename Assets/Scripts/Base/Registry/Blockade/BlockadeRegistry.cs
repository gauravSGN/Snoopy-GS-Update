using System;
using Service;
using System.Linq;
using System.Collections.Generic;

namespace Registry
{
    public class BlockadeRegistry : BlockadeService
    {
        private struct EventMapping
        {
            public readonly Action OnBlock;
            public readonly Action OnUnblock;

            public EventMapping(Action onBlock, Action onUnblock)
            {
                OnBlock = onBlock;
                OnUnblock = onUnblock;
            }
        }

        private readonly List<Blockade> blockades = new List<Blockade>();
        private readonly Dictionary<BlockadeType, EventMapping> mapping = new Dictionary<BlockadeType, EventMapping>
        {
            { BlockadeType.Popups, new EventMapping(Dispatch<BlockadeEvent.PopupsBlocked>,
                                                    Dispatch<BlockadeEvent.PopupsUnblocked>) },
            { BlockadeType.SceneChange, new EventMapping(Dispatch<BlockadeEvent.SceneChangeBlocked>,
                                                         Dispatch<BlockadeEvent.SceneChangeUnblocked>) },
            { BlockadeType.Input, new EventMapping(Dispatch<BlockadeEvent.InputBlocked>,
                                                   Dispatch<BlockadeEvent.InputUnblocked>) },
            { BlockadeType.Reactions, new EventMapping(Dispatch<BlockadeEvent.ReactionsBlocked>,
                                                       Dispatch<BlockadeEvent.ReactionsUnblocked>) },
        };

        public bool PopupsBlocked
        {
            get { return blockades.Any(b => (b.BlockadeType & BlockadeType.Popups) > 0); }
        }

        public bool SceneChangeBlocked
        {
            get { return blockades.Any(b => (b.BlockadeType & BlockadeType.SceneChange) > 0); }
        }

        public bool InputBlocked
        {
            get { return blockades.Any(b => (b.BlockadeType & BlockadeType.Input) > 0); }
        }

        public bool ReactionsBlocked
        {
            get { return blockades.Any(b => (b.BlockadeType & BlockadeType.Reactions) > 0); }
        }

        public void Add(Blockade blockade)
        {
            if (!blockades.Contains(blockade))
            {
                SendEvents(blockade, m => m.OnBlock);
                blockades.Add(blockade);
            }
        }

        public void Remove(Blockade blockade)
        {
            if (blockades.Remove(blockade))
            {
                SendEvents(blockade, m => m.OnUnblock);
            }
        }

        private static void Dispatch<T>() where T : GameEvent, new()
        {
            GlobalState.EventService.Dispatch<T>(new T());
        }

        private void SendEvents(Blockade blockade, Func<EventMapping, Action> handler)
        {
            var type = blockade.BlockadeType;

            foreach (var pair in mapping)
            {
                if (((type & pair.Key) > 0) && !blockades.Any(b => (b.BlockadeType & pair.Key) > 0))
                {
                    handler(pair.Value).Invoke();
                }
            }
        }
    }
}
