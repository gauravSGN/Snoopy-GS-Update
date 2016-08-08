using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core;

public class AddBubbleSpriteSwappers : MonoBehaviour
{
    [SerializeField]
    private GameObject blueSwapper;

    [SerializeField]
    private GameObject yellowSwapper;

    [SerializeField]
    private GameObject redSwapper;

    [SerializeField]
    private GameObject greenSwapper;

    [SerializeField]
    private GameObject pinkSwapper;

    [SerializeField]
    private GameObject purpleSwapper;

    [SerializeField]
    private float startupDelay = 0.5f;

    private BubbleType currentType;
    private List<Tuple<BubbleType, GameObject>> typePrefabCombos = new List<Tuple<BubbleType, GameObject>>(6);

    void Start()
    {
        StartCoroutine(DelayStart(startupDelay));
        typePrefabCombos.Add(new Tuple<BubbleType, GameObject>(BubbleType.Blue, blueSwapper));
        typePrefabCombos.Add(new Tuple<BubbleType, GameObject>(BubbleType.Yellow, yellowSwapper));
        typePrefabCombos.Add(new Tuple<BubbleType, GameObject>(BubbleType.Red, redSwapper));
        typePrefabCombos.Add(new Tuple<BubbleType, GameObject>(BubbleType.Green, greenSwapper));
        typePrefabCombos.Add(new Tuple<BubbleType, GameObject>(BubbleType.Pink, pinkSwapper));
        typePrefabCombos.Add(new Tuple<BubbleType, GameObject>(BubbleType.Purple, purpleSwapper));
    }

    private IEnumerator DelayStart(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var t in typePrefabCombos)
        {
            currentType = t.Item1;
            AddObjectToChildren.AddObject(transform, t.Item2, CheckColor);
        }
    }

    private bool CheckColor(GameObject go)
    {
        var attachments = go.GetComponent<BubbleAttachments>();
        return attachments.Model.type == currentType;
    }
}
