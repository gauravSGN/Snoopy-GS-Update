using UnityEngine;

namespace Util
{
    abstract public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        public static T Instance { get; private set; }

        virtual protected void Awake()
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
