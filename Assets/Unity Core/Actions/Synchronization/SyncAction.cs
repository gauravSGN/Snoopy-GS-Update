using UnityEngine;

namespace Actions.Synchronization
{
    sealed public class SyncAction : GameAction
    {
        sealed public class Semaphore
        {
            internal int count;
        }

        private Semaphore sharedObject;
        private bool locked;

        public bool Done { get { return sharedObject.count == 0; } }

        static public Semaphore CreateSharedObject()
        {
            return new Semaphore();
        }

        public SyncAction(Semaphore sharedObject)
        {
            this.sharedObject = sharedObject;
        }

        public void Attach(GameObject target)
        {
            Lock();
        }

        public void Detach(GameObject target)
        {
            Unlock();
        }

        public void Update()
        {
            Unlock();
        }

        private void Lock()
        {
            if (!locked)
            {
                sharedObject.count++;
                locked = true;
            }
        }

        private void Unlock()
        {
            if (locked)
            {
                sharedObject.count--;
                locked = false;
            }
        }
    }
}
