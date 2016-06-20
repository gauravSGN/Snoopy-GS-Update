using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
        private const string LEVEL_EXTENSION = "json";

        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private Transform levelContents;

        [SerializeField]
        private GameObject confirmationDialogPrefab;

        private string filename;

        public void New()
        {
            ConfirmAction(delegate ()
            {
                filename = null;
                manipulator.Clear();
            });
        }

        public void Open()
        {
            var filters = new[] { "Level Data", LEVEL_EXTENSION };
            var basePath = Path.Combine(Application.dataPath, "Data/Levels");
            var levelFilename = EditorUtility.OpenFilePanelWithFilters("Open Level", basePath, filters);

            if (!string.IsNullOrEmpty(levelFilename))
            {
                ConfirmAction(delegate ()
                {
                    filename = levelFilename;
                    manipulator.Clear();

                    manipulator.LoadLevel(File.ReadAllText(levelFilename));
                });
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(filename))
            {
                SaveAs();
            }
            else
            {
                ConfirmAction(delegate ()
                {
                    var jsonText = manipulator.SaveLevel();

                    using (var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(jsonText);
                    }
                });
            }
        }

        public void SaveAs()
        {
            var basePath = Path.Combine(Application.dataPath, "Data/Levels");
            var levelFilename = EditorUtility.SaveFilePanel("Save Level", basePath, "NewLevel.json", LEVEL_EXTENSION);

            if (!string.IsNullOrEmpty(levelFilename))
            {
                filename = levelFilename;
                Save();
            }
        }

        public void Clear()
        {
            ConfirmAction(delegate () { manipulator.Clear(); });
        }

        private void ConfirmAction(Action action)
        {
            // TODO: Remove this before finishing
            action.Invoke();
            return;

            var dialog = Instantiate(confirmationDialogPrefab).GetComponent<ConfirmationDialog>();
            dialog.transform.SetParent(transform.parent, false);

            dialog.Title = "Destructive Command";
            dialog.Body = "This action will overwrite the current working level data.  Are you sure you want to proceed?";
            dialog.OnConfirm = action;
        }
    }
}
