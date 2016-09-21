using gs;
using gs.persist;
using Data = System.Collections.Generic.IDictionary<string, object>;
using System.Collections.Generic;

namespace State
{
    public class PersistableStateHandler : StateHandler, Persistable
    {
        public uint PersistableVersion { get { return 1; } }

        override public void Initialize(Data topLevelState)
        {
            if (!initialized)
            {
                state = new Dictionary<string, object>();
                GS.Api.Persistence.register(this);
                initialized = true;
            }
        }

        // recover and persist implement the Persistable interface and thus will break our coding standard
        public void recover(string key, uint storedVersion, Data blob)
        {
            state = blob;
            NotifyListeners();
        }

        public Data persist(string key)
        {
            return state;
        }

        public void Save()
        {
            GS.Api.Persistence.flush(this);
        }

        protected void SaveAndNotifyListenersCallback(Observable target)
        {
            Save();
            NotifyListeners();
        }

        override protected void SetValue<T>(string key, object value)
        {
            state[key] = (T)value;
            SaveAndNotifyListenersCallback(null);
        }
    }
}