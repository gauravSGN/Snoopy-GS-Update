﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyOnFinish;

    [SerializeField]
    private float deathDelay;

    [SerializeField]
    private List<GameObject> activateOnPop;

    [SerializeField]
    private List<GameObject> activateOnCull;

    [SerializeField]
    private List<GameObject> deactivateOnDeath;

    private Dictionary<BubbleDeathType, List<IEnumerator>> effectDictionary;

    public IEnumerator TriggerDeathEffects(BubbleDeathType type)
    {
        var activateList = type == BubbleDeathType.Pop ? activateOnPop : activateOnCull;
        var effectController = gameObject.GetComponent<BubbleEffectController>();

        for (int i = 0, length = effectDictionary[type].Count; i < length; i++)
        {
            effectController.AddEffect(effectDictionary[type][i]);
        }

        for (int i = 0; i < activateList.Count; ++i)
        {
            activateList[i].SetActive(true);
        }

        for (int i = 0; i < deactivateOnDeath.Count; ++i)
        {
            deactivateOnDeath[i].SetActive(false);
        }

        yield return new WaitForSeconds(deathDelay);
        Destroy(destroyOnFinish);
    }

    public void AddEffect(IEnumerator effect, BubbleDeathType type)
    {
        effectDictionary[type].Add(effect);
    }

    protected void Awake()
    {
        effectDictionary = new Dictionary<BubbleDeathType, List<IEnumerator>>();
        effectDictionary[BubbleDeathType.Pop] = new List<IEnumerator>();
        effectDictionary[BubbleDeathType.Cull] = new List<IEnumerator>();
    }
}
