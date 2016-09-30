using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private Transform levelContents;

        [SerializeField]
        private GameObject confirmationDialogPrefab;

        [SerializeField]
        private GameObject returnPrefab;

        private string filename;
        private string basePath;

        private string Filename
        {
            get { return filename; }
            set
            {
                filename = value;
                UpdateBasePath();
            }
        }

        public void Start()
        {
            Filename = LevelEditorState.Instance.LevelFilename;

            if (!string.IsNullOrEmpty(LevelEditorState.Instance.LevelData))
            {
                manipulator.LoadLevel(LevelEditorState.Instance.LevelData);
            }
        }

        public void New()
        {
            ConfirmAction(delegate
            {
                Filename = null;
                manipulator.ClearAllPuzzles();
                GlobalState.EventService.Dispatch(new LevelEditorLoadEvent());
            });
        }

        public void Open()
        {
#if UNITY_EDITOR
            var filters = new[] { "Level Data", LevelEditorConstants.LEVEL_EXTENSION };
            var levelFilename = EditorUtility.OpenFilePanelWithFilters("Open Level", basePath, filters);

            if (!string.IsNullOrEmpty(levelFilename))
            {
                ConfirmAction(delegate
                {
                    Filename = levelFilename;
                    manipulator.ClearAllPuzzles();
                    manipulator.LoadLevel(File.ReadAllText(levelFilename));
                });
            }
#endif
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Filename))
            {
                SaveAs();
            }
            else
            {
                File.WriteAllText(Filename, manipulator.SaveLevel());
            }
        }

        public void SaveAs()
        {
#if UNITY_EDITOR
            var levelFilename = EditorUtility.SaveFilePanel(
                "Save Level",
                basePath,
                string.Format("NewLevel.{0}", LevelEditorConstants.LEVEL_EXTENSION),
                LevelEditorConstants.LEVEL_EXTENSION
            );

            if (!string.IsNullOrEmpty(levelFilename))
            {
                Filename = levelFilename;
                Save();
            }
#endif
        }

        public void Clear()
        {
            ConfirmAction(manipulator.ClearPuzzle);
        }

        public void TestLevel()
        {
            LevelEditorState.Instance.LevelFilename = Filename;
            LevelEditorState.Instance.LevelData = manipulator.SaveLevel();

            GlobalState.SceneService.NextLevelData = LevelEditorState.Instance.LevelData;
            GlobalState.SceneService.ReturnScene = LevelEditorConstants.SCENE_NAME;

            Instantiate(returnPrefab);

            GlobalState.SceneService.TransitionToScene(StringConstants.Scenes.LEVEL);
        }

        public void ConfirmAction(Action action)
        {
            var dialog = Instantiate(confirmationDialogPrefab).GetComponent<ConfirmationDialog>();
            dialog.transform.SetParent(transform.parent, false);

            dialog.Title = "Destructive Command";
            dialog.Body = "This action will modify the current working level data.  Are you sure you want to proceed?";
            dialog.OnConfirm = action;
        }

        private void UpdateBasePath()
        {
            if (!string.IsNullOrEmpty(Filename))
            {
                basePath = Path.GetDirectoryName(Filename);
            }
            else if (string.IsNullOrEmpty(basePath))
            {
                basePath = Path.Combine(Application.dataPath, LevelEditorConstants.LEVEL_BASE_PATH);
            }
        }
    }
}
