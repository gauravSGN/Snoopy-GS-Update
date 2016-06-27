using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class RowNumbers : GridScrollTracker
    {
        [SerializeField]
        private GameObject textPrefab;

        private float lastHeight;
        private readonly List<Text> elements = new List<Text>();

        override protected void Start()
        {
            base.Start();

            lastHeight = rectTransform.rect.height;

            CreateTextElements();
            UpdateTextElements();
        }

        override protected void LateUpdate()
        {
            if (Mathf.Abs(rectTransform.rect.height - lastHeight) > Mathf.Epsilon)
            {
                lastHeight = rectTransform.rect.height;
                CreateTextElements();
                UpdateTextElements();
            }
            else if (Mathf.Abs(ContentPosition - lastY) > Mathf.Epsilon)
            {
                UpdateTextElements();
            }

            base.LateUpdate();
        }

        private void CreateTextElements()
        {
            var elementCount = 1 + lastHeight / wrapHeight;

            while (elements.Count > elementCount)
            {
                Destroy(elements[elements.Count - 1].gameObject);
                elements.RemoveAt(elements.Count - 1);
            }

            while (elements.Count < elementCount)
            {
                var element = Instantiate(textPrefab);
                element.transform.SetParent(rectTransform, false);
                element.transform.localPosition += (Vector3.down * (elements.Count * wrapHeight));
                elements.Add(element.GetComponent<Text>());
            }
        }

        private void UpdateTextElements()
        {
            var topRow = (int)Mathf.Floor(ContentPosition / wrapHeight) + 1;

            for (var index = 0; index < elements.Count; index++)
            {
                elements[index].text = (topRow + index).ToString();
            }
        }
    }
}
