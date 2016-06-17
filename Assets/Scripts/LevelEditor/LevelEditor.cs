using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
        private const string LEVEL_EXTENSION = "xml";

        [SerializeField]
        private EditorLevelLoader loader;

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
                loader.Clear();
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
                    loader.Clear();

                    loader.LoadLevel(File.ReadAllText(levelFilename));
                });
            }
        }

        public void Save()
        {

        }

        public void SaveAs()
        {

        }

        public void Clear()
        {
            ConfirmAction(delegate () { loader.Clear(); });
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
