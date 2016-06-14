using UnityEditor;
using UnityEngine;
using System.IO;

public static class CustomAssetCreator
{
    [MenuItem("Assets/Create/Bubble Factory")]
    public static void CreateBubbleFactory()
    {
        CreateAThing<BubbleFactory>();
    }

    [MenuItem("Assets/Create/Bubble Definition")]
    public static void CreateBubbleDefinition()
    {
        CreateAThing<BubbleDefinition>();
    }

    [MenuItem("Assets/Create/Game Config")]
    public static void CreateGameConfig()
    {
        CreateAThing<GameConfig>();
    }

    private static void CreateAThing<T>() where T : ScriptableObject
    {
        var instance = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(instance, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = instance;
    }
}
