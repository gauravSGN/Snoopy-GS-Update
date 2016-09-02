using UnityEngine;
using Animation;
using Service;

public class AnimationProxy : MonoBehaviour, AnimationService
{
    [SerializeField]
    private AnimationFactory animationFactory;

    public GameObject CreateByType(AnimationType type)
    {
        return animationFactory.CreateByType(type);
    }

    public void Preload(AnimationType type, int count)
    {
        animationFactory.Preload(type, count);
    }

    protected void Start()
    {
        GlobalState.Instance.Services.SetInstance<AnimationService>(this);
    }
}
