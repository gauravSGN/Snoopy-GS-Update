using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using LevelEditor.Manipulator;
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
            eventService.AddEventHandler<RandomBubblesChangedEvent>(OnRandomBubblesChanged);
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

                DeleteAllGroups();
                manipulator.Randoms.Clear();
                manipulator.Randoms.AddRange(data.randoms);

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
            group.OnActivate += OnGroupActivate;
            group.Label = string.Format("R{0}", index + 1);
            group.Count = 0;
            groups.Add(group);
        }

        private void RemoveGroup(RandomBubbleGroup group)
        {
            var index = groups.IndexOf(group);

            DeleteGroup(index);
            definitions.RemoveAt(index);

            ResizeContents();
        }

        private void DeleteGroup(int index)
        {
            Destroy(groups[index].gameObject);
            groups.RemoveAt(index);
        }

        private void OnGroupActivate(RandomBubbleGroup group)
        {
            var index = groups.IndexOf(group);

            manipulator.SetActionType(ManipulatorActionType.PlaceBubbleAndModifier);
            manipulator.SetModifier(new BubbleModifierData
            {
                Type = BubbleModifierType.Random,
                Data = index.ToString(),
            });
        }

        private void OnRandomBubblesChanged(RandomBubblesChangedEvent gameEvent)
        {
            InitializeGroups();
        }

        private void InitializeGroups()
        {
            DeleteAllGroups();

            definitions = manipulator.Randoms;

            CreateAllGroups();

            if (definitions.Count < 1)
            {
                AddGroup();
            }

            ResizeContents();
        }

        private void DeleteAllGroups()
        {
            while (groups.Count > 0)
            {
                DeleteGroup(0);
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
