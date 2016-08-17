using gs;
using gs.data;
using UnityEngine;
using System.Collections.Generic;

namespace Config
{
    abstract public class GSDescriptor
    {
        public Dictionary<string, object> Contents { get; protected set; }

        abstract protected void InitializePlatformValues();

        public GSDescriptor(TextAsset jsonConfig)
        {
            Contents = JSON.Parse<Dictionary<string, object>>(jsonConfig.text);

            Contents["appVersion"] = Application.version;

            InitializePlatformValues();
        }

        public void Initialize()
        {
            GS.Init(Contents, () =>
            {
                GS.Api.SetIdentifier("store", (string)Contents["store"]);

                GS.Api.OnConfigChange += OnConfigChange;
                GS.Api.OnStateChange += OnStateChange;
                GS.Api.OnStorageLow += OnStorageLow;
                GS.Api.OnStorageAvailable += OnStorageAvailable;

                GlobalState.User.purchasables.hearts.Replenish();
                GlobalState.EventService.Dispatch(new GSInitializedEvent(this));
            });
        }

        public void OnConfigChange(object sender, System.EventArgs e)
        {
        }

        public void OnStateChange(object sender, System.EventArgs e)
        {
        }

        public void OnStorageLow(object sender, System.EventArgs e)
        {
        }

        public void OnStorageAvailable(object sender, System.EventArgs e)
        {
        }
    }
}
