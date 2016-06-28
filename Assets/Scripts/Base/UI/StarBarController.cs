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
    private List<Image> stars;

    private int[] scores = { 100, 600, 1000 };
    private int currentStar = 0;

    protected void Start()
    {
        PlaceStars();
        LevelStateUpdated(level.levelState);

        level.levelState.AddListener(LevelStateUpdated);
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
        var starCount = Mathf.Min(scores.Length, stars.Count);
        var score = level.levelState.score;

        for (var index = currentStar; index < starCount; index++)
        {
            if (score >= scores[index])
            {
                currentStar = index + 1;
                stars[index].sprite = activeStar;
                stars[index].SetNativeSize();
            }
        }

        fillImage.fillAmount = Mathf.Clamp01((float)score / (float)scores[starCount - 1]);
    }
}
