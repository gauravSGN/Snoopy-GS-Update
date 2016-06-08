using UnityEngine;
using System.Collections;

public class Lantern : MonoBehaviour
{
    public GameObject glow;
    public BubbleType bubbleType;
    public Level level;

    private int max = 0;
    private int current = 0;
    private float progress = 0;
    private int lastBubbleCount = 0;

    public void Setup(int setMax)
    {
        max = setMax;

        if (max > 0)
        {
            gameObject.SetActive(true);
            level.LevelState.AddListener(UpdateState);
        }
    }

    public void Reset()
    {
        glow.SetActive(false);
        progress = 0;
        current++;
    }

    private void UpdateState(LevelState levelState)
    {
        if (current < max && glow != null)
        {
            var currentBubbleCount = levelState.typeTotals[bubbleType];

            if (currentBubbleCount < lastBubbleCount)
            {
                var fillRate = ((float)(max - current) - progress) / currentBubbleCount;
                progress = Mathf.Min(1 , progress + ((lastBubbleCount - currentBubbleCount) * fillRate));
            }

            lastBubbleCount = currentBubbleCount;

            if (!glow.activeSelf && progress == 1)
            {
                glow.SetActive(true);
            }
        }
    }
}
