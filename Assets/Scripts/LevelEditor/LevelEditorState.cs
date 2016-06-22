using Util;

namespace LevelEditor
{
    public class LevelEditorState : SingletonBehaviour<LevelEditorState>
    {
        public string LevelFilename { get; set; }
        public string LevelData { get; set; }
    }
}
