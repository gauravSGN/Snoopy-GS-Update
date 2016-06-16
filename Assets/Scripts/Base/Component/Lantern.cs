using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField]
    private GameObject glow;

    [SerializeField]
    private BubbleType bubbleType;

    [SerializeField]
    private Level level;

    [SerializeField]
    private GameObject launcher;

    [SerializeField]
    private int max;

    [SerializeField]
    private int current;

    [SerializeField]
    private float progress;

    [SerializeField]
    private int lastBubbleCount;

    public void Setup(int setMax)
    {
        max = setMax;

        if (max > 0)
        {
            gameObject.SetActive(true);
            level.levelState.AddListener(UpdateState);
        }
    }

    protected void OnMouseUp()
    {
        if (progress >= 1.0f)
        {
            launcher.GetComponent<BubbleLauncher>().AddShotModifier(AddExplosion);
            Reset();
        }
    }

    public void AddExplosion(GameObject bubble)
    {
        bubble.AddComponent<BubbleExplode>();
        bubble.GetComponent<BubbleAttachments>().Model.type = BubbleType.Steel;
    }

    private void UpdateState(Observable levelState)
    {
        if ((current < max) && (glow != null))
        {
            var currentBubbleCount = (levelState as LevelState).typeTotals[bubbleType];

            if (currentBubbleCount < lastBubbleCount)
            {
                var fillRate = ((float)(max - current) - progress) / currentBubbleCount;
                progress = Mathf.Min(1.0f, progress + ((lastBubbleCount - currentBubbleCount) * fillRate));
            }

            lastBubbleCount = currentBubbleCount;

            if (!glow.activeSelf && (progress >= 1.0f))
            {
                glow.SetActive(true);
            }
        }
    }

    private void Reset()
    {
        glow.SetActive(false);
        progress = 0.0f;
        current++;
    }
}
