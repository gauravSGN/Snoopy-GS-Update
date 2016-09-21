using System;
using Effects;
using Animation;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarBarController : MonoBehaviour
{
    [SerializeField]
    private Level level;

    [SerializeField]
    private Image fillImage;

    [SerializeField]
    private ParticleSystem scoreBarIncreaseVFX;

    [SerializeField]
    private GameObject[] stars;

    private int[] scores;
    private int starCount;
    private int lastScore = -1;
    private int currentStar = 0;
    private float currentFillTime;

    protected void Start()
    {
        level.levelState.AddListener(LevelStateUpdated);
    }

    protected void OnDestroy()
    {
        level.levelState.RemoveListener(LevelStateUpdated);
    }

    private void PlaceStars()
    {
        float minX, maxX;

        minX = maxX = stars[0].transform.localPosition.x;

        for (var index = 0; index < starCount; index++)
        {
            var x = stars[index].transform.localPosition.x;

            minX = Mathf.Min(minX, x);
            maxX = Mathf.Max(maxX, x);
        }

        for (var index = 0; index < starCount; index++)
        {
            var oldPosition = stars[index].transform.localPosition;
            var portion = (float)scores[index] / (float)scores[starCount - 1];

            stars[index].transform.localPosition = new Vector3(minX + (maxX - minX) * portion, oldPosition.y);
        }
    }

    private void LevelStateUpdated(Observable state)
    {
        if (scores == null)
        {
            scores = level.levelState.starValues;
            starCount = Math.Min(scores.Length, stars.Length);
            PlaceStars();
        }

        var score = level.levelState.score;

        if (score > lastScore)
        {
            lastScore = score;

            UpdateStarImages();
            StartCoroutine(UpdateFillImage());
        }
    }

    private void UpdateStarImages()
    {
        for (var index = currentStar; index < starCount; index++)
        {
            if (lastScore >= scores[index])
            {
                currentStar = index + 1;
                StartCoroutine(AnimationEffect.Play(stars[index], AnimationType.ActivateStarBarStar));
            }
        }
    }

    private IEnumerator UpdateFillImage()
    {
        if (currentFillTime <= 0.01f)
        {
            var timeToFill = scoreBarIncreaseVFX.duration;
            var vfxTransform = scoreBarIncreaseVFX.transform;
            var fillImageWidth = fillImage.rectTransform.rect.width;

            while (currentFillTime < timeToFill)
            {
                currentFillTime += Time.deltaTime;

                var lastFillAmount = fillImage.fillAmount;
                var endFillAmount = Mathf.Clamp01((float)lastScore / (float)scores[starCount - 1]);
                var newFillAmount = Mathf.Lerp(lastFillAmount, endFillAmount, (currentFillTime / timeToFill));

                fillImage.fillAmount = newFillAmount;

                if (newFillAmount > lastFillAmount)
                {
                    vfxTransform.localPosition = new Vector3((newFillAmount * fillImageWidth),
                                                             vfxTransform.localPosition.y);

                    if (!scoreBarIncreaseVFX.isPlaying)
                    {
                        scoreBarIncreaseVFX.Play();
                    }
                }

                yield return null;
            }

            currentFillTime = 0.0f;
        }
    }
}
