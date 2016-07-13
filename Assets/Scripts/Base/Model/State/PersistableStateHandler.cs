using System;
using gs;
using gs.persist;
using Data = System.Collections.Generic.IDictionary<string, object>;
using System.Collections.Generic;


using UnityEngine;

namespace State
{
    public class PersistableStateHandler : StateHandler, Persistable
    {
        private Persistence persistence;

        public uint PersistableVersion { get { return 1; } }

        public PersistableStateHandler(Data topLevelState, Action<Observable> initialListener = null) : base(topLevelState, initialListener)
        {
            state = new Dictionary<string, object>();
            InitializeStateKeys();

            persistence = GS.Api.Persistence;
            persistence.register(this);
        }

        // recover and persist implement the Persistable interface and thus will break our coding standard
        public void recover(string key, uint storedVersion, Data blob)
        {
            state = blob;
            InitializeStateKeys();
            NotifyListeners();
        }

        public Data persist(string key)
        {
            return state;
        }

        public void Save()
        {
            persistence.flush(this);
        }
    }
}