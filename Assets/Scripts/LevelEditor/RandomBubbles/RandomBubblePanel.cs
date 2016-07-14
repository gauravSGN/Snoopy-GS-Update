using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Service;
using Model;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LevelEditor
{
    public class RandomBubblePanel : MonoBehaviour
    {
        [Serializable]
        private sealed class RandomsFileWrapper
        {
            public RandomBubbleDefinition[] randoms;
        }

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

        public void SaveToFile()
        {
#if UNITY_EDITOR
            var basePath = Path.Combine(Application.dataPath, LevelEditorConstants.RANDOMS_BASE_PATH);
            var filename = EditorUtility.SaveFilePanel(
                "Save Randoms",
                basePath,
                "NewRandoms.json",
                LevelEditorConstants.RANDOMS_EXTENSION);

            if (!string.IsNullOrEmpty(filename))
            {
                var data = new RandomsFileWrapper();
                data.randoms = definitions.ToArray();

                using (var file = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Write))
                using (var stream = new StreamWriter(file))
                {
                    stream.WriteLine(JsonUtility.ToJson(data));
                }
            }
#endif
        }

        public void LoadFromFile()
        {
#if UNITY_EDITOR
            var filters = new[] { "Randoms Data", LevelEditorConstants.RANDOMS_EXTENSION };
            var basePath = Path.Combine(Application.dataPath, LevelEditorConstants.RANDOMS_BASE_PATH);
            var filename = EditorUtility.OpenFilePanelWithFilters("Open Randoms", basePath, filters);

            if (!string.IsNullOrEmpty(filename))
            {
                var data = JsonUtility.FromJson<RandomsFileWrapper>(File.ReadAllText(filename));

                RemoveAllGroups();

                foreach (var definition in data.randoms)
                {
                    definitions.Add(definition);
                }

                CreateAllGroups();
                ResizeContents();
            }
#endif
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
            RemoveAllGroups();

            definitions = manipulator.Randoms;

            CreateAllGroups();

            if (definitions.Count < 1)
            {
                AddGroup();
            }

            ResizeContents();
        }

        private void RemoveAllGroups()
        {
            while (groups.Count > 0)
            {
                RemoveGroup(groups[0]);
            }
        }

        private void CreateAllGroups()
        {
            while (groups.Count < definitions.Count)
            {
                CreateGroup();
            }
        }

        private void ResizeContents()
        {
            contents.sizeDelta = new Vector2(0.0f, rowHeight * groups.Count);
        }
    }
}
