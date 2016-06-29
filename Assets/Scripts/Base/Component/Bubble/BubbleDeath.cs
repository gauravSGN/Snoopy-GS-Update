using UnityEngine;
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

    public enum DeathType
    {
        Pop = 0,
        Cull = 1
    }

    public IEnumerator TriggerDeathEffects(DeathType type)
    {
        var activateList = type == DeathType.Pop ? activateOnPop : activateOnCull;

        for (int i = 0; i < activateList.Count; ++i)
        {
            Debug.Log(i);
            activateList[i].SetActive(true);
        }

        for (int i = 0; i < deactivateOnDeath.Count; ++i)
        {
            deactivateOnDeath[i].SetActive(false);
        }

        yield return new WaitForSeconds(deathDelay);
        Destroy(destroyOnFinish);
    }

    public void AddPopEffect(GameObject popEffect)
    {
        activateOnPop.Add(popEffect);
    }
}
