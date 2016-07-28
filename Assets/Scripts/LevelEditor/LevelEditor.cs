using System;
using System.IO;
using UnityEngine;
using LevelEditor.Manipulator;
using UnityEngine.SceneManagement;
using Service;

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

        public void Start()
        {
            filename = LevelEditorState.Instance.LevelFilename;

            if (!string.IsNullOrEmpty(LevelEditorState.Instance.LevelData))
            {
                manipulator.LoadLevel(LevelEditorState.Instance.LevelData);
            }
        }

        public void New()
        {
            ConfirmAction(delegate
            {
                filename = null;
                ClearBoard();
                GlobalState.Instance.Services.Get<Service.EventService>().Dispatch(new LevelEditorLoadEvent());
            });
        }

        public void Open()
        {
#if UNITY_EDITOR
            var filters = new[] { "Level Data", LevelEditorConstants.LEVEL_EXTENSION };
            var basePath = Path.Combine(Application.dataPath, LevelEditorConstants.LEVEL_BASE_PATH);
            var levelFilename = EditorUtility.OpenFilePanelWithFilters("Open Level", basePath, filters);

            if (!string.IsNullOrEmpty(levelFilename))
            {
                ConfirmAction(delegate ()
                {
                    filename = levelFilename;
                    ClearBoard();

                    manipulator.LoadLevel(File.ReadAllText(levelFilename));
                });
            }
#endif
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(filename))
            {
                SaveAs();
            }
            else
            {
                File.WriteAllText(filename, manipulator.SaveLevel());
            }
        }

        public void SaveAs()
        {
#if UNITY_EDITOR
            var basePath = Path.Combine(Application.dataPath, LevelEditorConstants.LEVEL_BASE_PATH);
            var levelFilename = EditorUtility.SaveFilePanel(
                "Save Level",
                basePath,
                string.Format("NewLevel.{0}", LevelEditorConstants.LEVEL_EXTENSION),
                LevelEditorConstants.LEVEL_EXTENSION
            );

            if (!string.IsNullOrEmpty(levelFilename))
            {
                filename = levelFilename;
                Save();
            }
#endif
        }

        public void Clear()
        {
            ConfirmAction(ClearBoard);
        }

        public void TestLevel()
        {
            LevelEditorState.Instance.LevelFilename = filename;
            LevelEditorState.Instance.LevelData = manipulator.SaveLevel();

            var sceneData = GlobalState.Instance.Services.Get<SceneService>();
            sceneData.NextLevelData = LevelEditorState.Instance.LevelData;
            sceneData.ReturnScene = LevelEditorConstants.SCENE_NAME;

            Instantiate(returnPrefab);

            SceneManager.LoadScene("Level");
        }

        public void ConfirmAction(Action action)
        {
            var dialog = Instantiate(confirmationDialogPrefab).GetComponent<ConfirmationDialog>();
            dialog.transform.SetParent(transform.parent, false);

            dialog.Title = "Destructive Command";
            dialog.Body = "This action will modify the current working level data.  Are you sure you want to proceed?";
            dialog.OnConfirm = action;
        }

        private void ClearBoard()
        {
            GlobalState.Instance.Services.Get<EventService>().Dispatch(new ClearLevelEvent());
            var clearAction = manipulator.ActionFactory.Create(ManipulatorActionType.Clear);
            clearAction.Perform(manipulator, 0, 0);
        }
    }
}
