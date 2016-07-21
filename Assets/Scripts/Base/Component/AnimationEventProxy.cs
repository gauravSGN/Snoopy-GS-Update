using UnityEngine;
using System;

public class AnimationEventProxy : MonoBehaviour
{
    public event Action OnAnimationFire;

    public void AnimationFire()
    {
        if (OnAnimationFire != null)
        {
            OnAnimationFire();
        }
    }
}
