using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Model;
using Util;
using System;
using System.Linq;

namespace LevelEditor.Menu
{
    public class BubbleActionMenu : MonoBehaviour
    {
        [SerializeField]
        private RectTransform menuContainer;

        [SerializeField]
        private LevelManipulator manipulator;

        private readonly List<MenuWidget> widgets = new List<MenuWidget>();

        public void Show()
        {
            Vector2 clickPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(),
                Input.mousePosition,
                null,
                out clickPosition
            );

            menuContainer.localPosition = clickPosition;

            gameObject.SetActive(true);
        }

        public void Populate(BubbleData data)
        {
            var layoutGroup = menuContainer.GetComponent<VerticalLayoutGroup>();

            if (widgets.Count == 0)
            {
                DiscoverWidgets();
            }

            var horizontalPadding = layoutGroup.padding.left + layoutGroup.padding.right;
            var items = new List<GameObject>();
            float height = layoutGroup.padding.top + layoutGroup.padding.bottom;
            float width = horizontalPadding;

            RemoveAllMenuChildren();

            foreach (var widget in widgets.Where(w => w.IsValidFor(data)))
            {
                var item = widget.CreateWidget(data);
                var rectTransform = item.GetComponent<RectTransform>();
                var rect = rectTransform.rect;

                rectTransform.SetParent(menuContainer, false);
                height += rect.height;
                width = Mathf.Max(width, rect.width + horizontalPadding);

                items.Add(item);
            }

            height += Mathf.Max(0, (items.Count - 1)) * layoutGroup.spacing;

            menuContainer.sizeDelta = new Vector2(width, height);
        }

        private void RemoveAllMenuChildren()
        {
            var menuTransform = menuContainer.transform;

            for (var index = menuTransform.childCount - 1; index >= 0; index--)
            {
                Destroy(menuTransform.GetChild(index).gameObject);
            }
        }

        private void DiscoverWidgets()
        {
            foreach (var widgetType in ReflectionUtil.GetDerivedTypes<MenuWidget>())
            {
                widgets.Add(Activator.CreateInstance(widgetType) as MenuWidget);
                widgets[widgets.Count - 1].Manipulator = manipulator;
                widgets[widgets.Count - 1].OnWidgetComplete += OnWidgetComplete;
            }

            widgets.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
        }

        private void OnWidgetComplete()
        {
            gameObject.SetActive(false);
        }
    }
}
