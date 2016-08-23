using UnityEngine;
using System.Collections.Generic;

namespace Snoopy.Characters
{
    sealed public class StartFlyDownHandler : MonoBehaviour
    {
        public void StartFlyDown()
        {
            transform.parent.GetComponent<WoodstockEventHandler>().StartFlyDown();
        }
    }
}
