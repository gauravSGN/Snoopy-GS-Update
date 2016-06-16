using UnityEngine;
using UnityEditor;
using System.IO;

namespace LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
        private const string LEVEL_EXTENSION = "xml";

        [SerializeField]
        private LevelLoader loader;

        [SerializeField]
        private Transform levelContents;

        private string filename;

        public void New()
        {
            filename = null;
            ClearLevel();
        }

        public void Open()
        {
            var filters = new[] { "Level Data", LEVEL_EXTENSION };
            var basePath = Path.Combine(Application.dataPath, "Data/Levels");
            var levelFilename = EditorUtility.OpenFilePanelWithFilters("Open Level", basePath, filters);

            if (!string.IsNullOrEmpty(levelFilename))
            {
                filename = levelFilename;
                loader.LoadLevel(File.ReadAllText(levelFilename));
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

        }

        private void ClearLevel()
        {
            for (var index = levelContents.childCount - 1; index >= 0; index--)
            {
                Destroy(levelContents.GetChild(index).gameObject);
            }
        }
    }
}
