using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace LevelEditor
{
    sealed public class RandomBubbleExclusions : MonoBehaviour
    {
        [SerializeField]
        private Text caption;

        [SerializeField]
        private GameObject itemsPanel;

        [SerializeField]
        private RectTransform contents;

        [SerializeField]
        private GameObject itemTemplate;

        private readonly List<GameObject> itemInstances = new List<GameObject>();
        private Vector3 originalPosition;
        private Transform originalParent;
        private List<int> items;
        private List<int> options;

        public void Initialize(List<int> items)
        {
            this.items = items;
            UpdateCaption();
        }

        public void UpdateOptions(List<int> options)
        {
            this.options = options;

            items.RemoveAll(i => !options.Contains(i));
            UpdateCaption();
        }

        public void Toggle()
        {
            var active = itemsPanel.activeSelf;

            var canvas = GetComponentInParent<Canvas>();
            var modalOverlay = canvas.transform.FindChild("ModalOverlay");
            var modalButton = modalOverlay.GetComponent<Button>();

            if (!active)
            {
                RebuildItems();

                originalParent = itemsPanel.transform.parent;
                originalPosition = itemsPanel.transform.localPosition;
                itemsPanel.transform.SetParent(modalOverlay.transform, true);

                modalButton.onClick.AddListener(Toggle);
                modalOverlay.gameObject.SetActive(true);
            }
            else
            {
                itemsPanel.transform.SetParent(originalParent, true);
                itemsPanel.transform.localPosition = originalPosition;

                modalButton.onClick.RemoveListener(Toggle);

                ReadResults();
            }

            itemsPanel.SetActive(!active);
        }

        private void RebuildItems()
        {
            CreateInstances();

            var count = options.Count;

            for (var index = 0; index < count; index++)
            {
                var instance = itemInstances[index];
                instance.GetComponentInChildren<Text>().text = (options[index] + 1).ToString();
                instance.GetComponentInChildren<Toggle>().isOn = items.Contains(options[index]);
            }
        }

        private void CreateInstances()
        {
            var count = options.Count;

            while (itemInstances.Count > count)
            {
                Destroy(itemInstances[itemInstances.Count - 1]);
                itemInstances.RemoveAt(itemInstances.Count - 1);
            }

            while (itemInstances.Count < count)
            {
                var instance = Instantiate(itemTemplate);
                instance.transform.SetParent(contents, false);
                instance.SetActive(true);
                itemInstances.Add(instance);
            }

            contents.sizeDelta = new Vector2(contents.sizeDelta.x, 24.0f * count);
        }

        private void ReadResults()
        {
            items.Clear();

            var count = Mathf.Min(options.Count, itemInstances.Count);
            for (var index = 0; index < count; index++)
            {
                if (itemInstances[index].GetComponentInChildren<Toggle>().isOn)
                {
                    items.Add(options[index]);
                }
            }

            items.RemoveAll(i => !options.Contains(i));
            UpdateCaption();
        }

        private void UpdateCaption()
        {
            caption.text = string.Join(",", items.Select(i => (i + 1).ToString()).ToArray());
        }
    }
}
