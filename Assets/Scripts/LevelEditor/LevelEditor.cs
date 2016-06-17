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
        private LevelLoader loader;

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
                ClearLevel();
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
                    ClearLevel();

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
            ConfirmAction(delegate ()
            {
                ClearLevel();
            });
        }

        private void ClearLevel()
        {
            for (var index = levelContents.childCount - 1; index >= 0; index--)
            {
                Destroy(levelContents.GetChild(index).gameObject);
            }
        }

        private void ConfirmAction(Action action)
        {
            var dialog = Instantiate(confirmationDialogPrefab).GetComponent<ConfirmationDialog>();
            dialog.transform.SetParent(transform.parent, false);

            dialog.Title = "Destructive Command";
            dialog.Body = "This action will overwrite the current working level data.  Are you sure you want to proceed?";
            dialog.OnConfirm = action;
        }
    }
}
