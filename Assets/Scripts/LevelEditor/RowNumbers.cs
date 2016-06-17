using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class RowNumbers : MonoBehaviour
{
    private const float ROW_SIZE = 32 * MathUtil.COS_30_DEGREES;

    private float ContentPosition { get { return content.localPosition.y; } }

    [SerializeField]
    private RectTransform content;

    [SerializeField]
    private GameObject textPrefab;

    private RectTransform rectTransform;
    private float lastY;
    private float lastHeight;
    private readonly List<Text> elements = new List<Text>();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        lastY = ContentPosition;
        lastHeight = rectTransform.rect.height;

        CreateTextElements();
        UpdateTextElements();
    }

    private void Update()
    {
        if (Mathf.Abs(rectTransform.rect.height - lastHeight) > Mathf.Epsilon)
        {
            lastHeight = rectTransform.rect.height;
            CreateTextElements();
        }

        if (Mathf.Abs(ContentPosition - lastY) > Mathf.Epsilon)
        {
            lastY = ContentPosition;
            UpdateTextElements();
            UpdatePosition();
        }
    }

    private void CreateTextElements()
    {
        var elementCount = 1 + lastHeight / ROW_SIZE;

        while (elements.Count > elementCount)
        {
            Destroy(elements[elements.Count - 1].gameObject);
            elements.RemoveAt(elements.Count - 1);
        }

        while (elements.Count < elementCount)
        {
            var element = Instantiate(textPrefab);
            element.transform.SetParent(rectTransform, false);
            element.transform.localPosition += (Vector3.down * (elements.Count * ROW_SIZE));
            elements.Add(element.GetComponent<Text>());
        }
    }

    private void UpdateTextElements()
    {
        var topRow = (int)Mathf.Round(lastY / ROW_SIZE - 0.5f);

        for (var index = 0; index < elements.Count; index++)
        {
            elements[index].text = (topRow + index).ToString();
        }
    }

    private void UpdatePosition()
    {
        var oldPosition = rectTransform.localPosition;
        rectTransform.localPosition = new Vector3(
            oldPosition.x,
            lastY % ROW_SIZE,
            oldPosition.z
        );
    }
}
