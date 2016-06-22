using UnityEngine;

namespace Util
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = (T)this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
