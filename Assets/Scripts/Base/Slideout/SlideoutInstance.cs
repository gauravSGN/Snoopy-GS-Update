using System;
using UnityEngine;
using System.Collections.Generic;

namespace Slideout
{
    sealed public class SlideoutInstance : MonoBehaviour
    {
        public event Action<SlideoutInstance> OnComplete;

        public void OnDestroy()
        {
            if (OnComplete != null)
            {
                OnComplete(this);
            }
        }
    }
}
