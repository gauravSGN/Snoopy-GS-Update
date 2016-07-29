using Util;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class BaseMoveCamera : MonoBehaviour
{
    [SerializeField]
    protected GameObject gameView;

    [SerializeField]
    protected float panSpeed;

    [SerializeField]
    protected float startDelay;

    [SerializeField]
    protected List<GameObject> disableOnMove;

    protected Collider2D castingBox;

    abstract protected IEnumerator MoveGameView();

    virtual protected void Start()
    {
        castingBox = GetComponent<Collider2D>();
    }

    protected bool IsTouchingBubbles()
    {
        return CastingUtil.BoundsBoxCast(castingBox.bounds, 1 << (int)Layers.GameObjects).Length > 0;
    }
}