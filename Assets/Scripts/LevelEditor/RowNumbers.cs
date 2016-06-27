using System.Collections.Generic;
using LevelEditor.Manipulator;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class RowNumbers : GridScrollTracker
    {
        [SerializeField]
        private GameObject textPrefab;

        [SerializeField]
        private LevelManipulator manipulator;

        private float lastHeight;
        private readonly List<Text> elements = new List<Text>();
        private readonly InsertRowAction action = new InsertRowAction();

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
                CreateTextElement();
            }
        }

        private void CreateTextElement()
        {
            var element = Instantiate(textPrefab);

            element.transform.SetParent(rectTransform, false);
            element.transform.localPosition += (Vector3.down * (elements.Count * wrapHeight));
            elements.Add(element.GetComponent<Text>());

            var buttons = element.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(() => RemoveRowHandler(element.transform));
            buttons[1].onClick.AddListener(() => InsertRowHandler(element.transform));
        }

        private void UpdateTextElements()
        {
            var topRow = (int)Mathf.Floor(ContentPosition / wrapHeight) + 1;

            for (var index = 0; index < elements.Count; index++)
            {
                elements[index].text = (topRow + index).ToString();
            }
        }

        private void RemoveRowHandler(Transform element)
        {
            var y = int.Parse(element.GetComponent<Text>().text) - 1;
            action.PerformAlternate(manipulator, 0, y);
        }

        private void InsertRowHandler(Transform element)
        {
            var y = int.Parse(element.GetComponent<Text>().text) - 1;
            action.Perform(manipulator, 0, y);
        }
    }
}
