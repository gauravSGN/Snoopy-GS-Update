using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StarBarController : MonoBehaviour
{
    [SerializeField]
    private Level level;

    [SerializeField]
    private Image fillImage;

    [SerializeField]
    private Sprite inactiveStar;

    [SerializeField]
    private Sprite activeStar;

    [SerializeField]
    private ParticleSystem scoreBarIncreaseVFX;

    [SerializeField]
    private List<Image> stars;

    private int[] scores;
    private int lastScore = -1;
    private int currentStar = 0;

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
        var starCount = Mathf.Min(scores.Length, stars.Count);
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
            PlaceStars();
        }

        var score = level.levelState.score;

        if (score > lastScore)
        {
            lastScore = score;

            var starCount = Mathf.Min(scores.Length, stars.Count);

            UpdateFillImage(score, starCount);
            UpdateStarImages(score, starCount);
        }
    }

    private void UpdateStarImages(int score, int starCount)
    {
        for (var index = currentStar; index < starCount; index++)
        {
            if (score >= scores[index])
            {
                currentStar = index + 1;
                stars[index].sprite = activeStar;
                stars[index].SetNativeSize();
            }
        }
    }

    private void UpdateFillImage(int score, int starCount)
    {
        var lastFillAmount = fillImage.fillAmount;
        var newFillAmount = Mathf.Clamp01((float)score / (float)scores[starCount - 1]);

        fillImage.fillAmount = newFillAmount;

        if ((newFillAmount > lastFillAmount) || (newFillAmount >= 1.0))
        {
            var vfxTransform = scoreBarIncreaseVFX.transform;
            var newX = (newFillAmount * fillImage.rectTransform.rect.width);

            vfxTransform.localPosition = new Vector3(newX, vfxTransform.localPosition.y);

            scoreBarIncreaseVFX.Play();
        }
    }
}
