using UnityEngine;
using System.Collections.Generic;
using Service;
using Model;

namespace LevelEditor
{
    public class RandomBubblePanel : MonoBehaviour
    {
        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private GameObject groupPrefab;

        [SerializeField]
        private RectTransform contents;

        [SerializeField]
        private float rowHeight;

        private List<RandomBubbleDefinition> definitions;
        private List<RandomBubbleGroup> groups = new List<RandomBubbleGroup>();

        public void Start()
        {
            InitializeGroups();

            var eventService = GlobalState.Instance.Services.Get<EventService>();
            eventService.AddEventHandler<LevelEditorLoadEvent>(OnLevelEditorLoad);
        }

        public void AddGroup()
        {
            definitions.Add(new RandomBubbleDefinition());
            CreateGroup();

            ResizeContents();
        }

        private void CreateGroup()
        {
            var index = groups.Count;
            var instance = Instantiate(groupPrefab);
            var group = instance.GetComponent<RandomBubbleGroup>();

            instance.transform.SetParent(contents, false);

            group.Initialize(manipulator.BubbleFactory, definitions[index]);
            group.DeleteButton.onClick.AddListener(() => RemoveGroup(group));
            group.Label = string.Format("R{0}", index);
            group.Count = 0;
            groups.Add(group);
        }

        private void RemoveGroup(RandomBubbleGroup group)
        {
            var index = groups.IndexOf(group);
            Destroy(group.gameObject);

            definitions.RemoveAt(index);
            groups.RemoveAt(index);

            ResizeContents();
        }

        private void OnLevelEditorLoad(LevelEditorLoadEvent gameEvent)
        {
            InitializeGroups();
        }

        private void InitializeGroups()
        {
            while (groups.Count > 0)
            {
                RemoveGroup(groups[0]);
            }

            definitions = manipulator.Randoms;

            while (groups.Count < definitions.Count)
            {
                CreateGroup();
            }

            if (definitions.Count < 1)
            {
                AddGroup();
            }

            ResizeContents();
        }

        private void ResizeContents()
        {
            contents.sizeDelta = new Vector2(0.0f, rowHeight * groups.Count);
        }
    }
}
