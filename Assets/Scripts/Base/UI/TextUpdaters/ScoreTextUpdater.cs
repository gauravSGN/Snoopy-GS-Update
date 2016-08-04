using UnityEngine;
using System.Collections;

public class ScoreTextUpdater : LevelStateTextUpdater
{
    private IEnumerator rollup;

    override protected void UpdateText(Observable target)
    {
        if ((text != null) && (target != null))
        {
            if (rollup != null)
            {
                StopCoroutine(rollup);
            }

            rollup = RollupCoroutine();
            StartCoroutine(rollup);
        }
    }

    private IEnumerator RollupCoroutine()
    {
        var config = GlobalState.Instance.Config.scoring;
        var startValue = int.Parse(text.text);
        var endValue = int.Parse(BuildString());
        var timer = 0.0f;
        float value;

        while (timer < config.rollupDuration)
        {
            timer += Time.deltaTime;

            value = Mathf.Max(0.0f, Mathf.Min(1.0f, timer / config.rollupDuration));
            value = config.rollup.Evaluate(value);

            text.text = ((int)Mathf.Round(startValue + (endValue - startValue) * value)).ToString();

            yield return null;
        }

        rollup = null;
    }
}
