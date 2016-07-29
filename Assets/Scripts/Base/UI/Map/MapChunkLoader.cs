using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map
{
    [RequireComponent(typeof(LayoutElement))]
    public class MapChunkLoader : MonoBehaviour
    {
        private const int CHUNKS_TO_BUFFER = 1;

        [SerializeField]
        private bool loadOnStart;

        [SerializeField]
        private MapChunkFactory mapChunkFactory;

        [SerializeField]
        private MapChunkType mapType;

        private GameObject mapChunk;

        public int MapChunkID { get { return transform.GetSiblingIndex(); } }

        public void Load()
        {
            if (mapChunk == null)
            {
                mapChunk = mapChunkFactory.CreateByType(mapType);
                mapChunk.transform.SetParent(gameObject.transform);
                mapChunk.transform.localPosition = Vector3.zero;
                mapChunk.transform.localScale = new Vector3(1.0f, 1.0f);
            }
        }

        public void Unload()
        {
            if (mapChunk != null)
            {
                Destroy(mapChunk);
                mapChunk = null;
            }
        }

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<LoadMapChunksEvent>(OnLoadChunksEvent);
            GlobalState.EventService.AddEventHandler<UnloadMapChunksEvent>(OnUnloadChunksEvent);

            if (loadOnStart)
            {
                Load();
            }
        }

        protected void OnTriggerEnter2D(Collider2D collider)
        {
            DispatchChunkEvent<UnloadMapChunksEvent>(new int[] { (MapChunkID - CHUNKS_TO_BUFFER - 1),
                                                                 (MapChunkID + CHUNKS_TO_BUFFER + 1) });

            DispatchChunkEvent<LoadMapChunksEvent>(Enumerable.Range((MapChunkID - CHUNKS_TO_BUFFER),
                                                                    (CHUNKS_TO_BUFFER * 2) + 1).ToArray());
        }

        private void OnLoadChunksEvent(LoadMapChunksEvent gameEvent)
        {
            if (gameEvent.mapChunkIDs.Contains(MapChunkID))
            {
                Load();
            }
        }

        private void OnUnloadChunksEvent(UnloadMapChunksEvent gameEvent)
        {
            if (gameEvent.mapChunkIDs.Contains(MapChunkID))
            {
                Unload();
            }
        }

        private void DispatchChunkEvent<T>(int[] chunkIDs) where T : BaseMapChunkEvent
        {
            T chunkEvent = GlobalState.EventService.GetPooledEvent<T>();
            chunkEvent.mapChunkIDs = chunkIDs;
            GlobalState.EventService.DispatchPooled<T>(chunkEvent);
        }
    }
}