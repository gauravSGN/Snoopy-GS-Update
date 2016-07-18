using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace LevelEditor
{
    public class RandomExclusionPanel : MonoBehaviour
    {
        [SerializeField]
        private Button addButton;

        [SerializeField]
        private RectTransform contents;

        [SerializeField]
        private GameObject elementPrefab;

        private List<InputField> elements = new List<InputField>();
        private List<int> data;

        public void Start()
        {
            addButton.onClick.AddListener(OnAddClick);
        }

        public void Initialize(List<int> data)
        {
            this.data = data;
            var count = data.Count;

            while (elements.Count > count)
            {
                RemoveElement(count);
            }

            while (elements.Count < count)
            {
                CreateElement();
            }

            for (var index = 0; index < count; index++)
            {
                elements[index].text = data[index].ToString();
            }

            ReorderElements();
        }

        private void OnAddClick()
        {
            data.Add(0);
            CreateElement();
            ReorderElements();
        }

        private void CreateElement()
        {
            var instance = Instantiate(elementPrefab);
            instance.transform.SetParent(contents, false);

            var element = instance.GetComponent<InputField>();
            element.onEndEdit.AddListener(OnEndEdit);
            elements.Add(element);
        }

        private void RemoveElement(int index)
        {
            Destroy(elements[index].gameObject);
            elements.RemoveAt(index);
        }

        private void ReorderElements()
        {
            var count = elements.Count;

            for (var index = 0; index < count; index++)
            {
                elements[index].transform.SetSiblingIndex(index);
            }

            addButton.transform.SetSiblingIndex(count);
        }

        private void OnEndEdit(string value)
        {
            for (var index = 0; index < elements.Count;)
            {
                if (elements[index].text.Trim() == "")
                {
                    RemoveElement(index);
                }
                else
                {
                    try
                    {
                        data[index] = int.Parse(elements[index].text);
                    }
                    catch (System.FormatException)
                    {
                        data[index] = 0;
                        elements[index].text = data[index].ToString();
                    }

                    index++;
                }
            }

            if (data.Count != elements.Count)
            {
                data.RemoveRange(elements.Count, data.Count - elements.Count);
            }
        }
    }
}
