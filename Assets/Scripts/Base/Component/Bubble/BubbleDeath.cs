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
    private List<GameObject> activateOnDeath;

    [SerializeField]
    private List<GameObject> deactivateOnDeath;

    public IEnumerator TriggerDeathEffects()
    {
        for(int i = 0; i < activateOnDeath.Count; ++i)
        {
            activateOnDeath[i].SetActive(true);
        }
        for(int i = 0; i < deactivateOnDeath.Count; ++i)
        {
            deactivateOnDeath[i].SetActive(false);
        }
        yield return new WaitForSeconds(deathDelay);
        Destroy(destroyOnFinish);
    }
}
