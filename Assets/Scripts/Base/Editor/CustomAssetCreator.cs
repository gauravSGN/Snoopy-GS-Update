using Model;
using UI.Map;
using UI.Popup;
using PowerUps;
using Animation;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CustomAssetCreator
{
    [MenuItem("Assets/Create/Bubbles/Bubble Factory")]
    public static void CreateBubbleFactory()
    {
        CreateAThing<BubbleFactory>();
    }

    [MenuItem("Assets/Create/Bubbles/Bubble Definition")]
    public static void CreateBubbleDefinition()
    {
        CreateAThing<BubbleDefinition>();
    }

    [MenuItem("Assets/Create/Bubbles/Bubble Modifier")]
    public static void CreateBubbleModifierDefinition()
    {
        CreateAThing<BubbleModifierDefinition>();
    }

    [MenuItem("Assets/Create/Game Config")]
    public static void CreateGameConfig()
    {
        CreateAThing<GameConfig>();
    }

    [MenuItem("Assets/Create/Power Up/Power Up Definition")]
    public static void CreatePowerUpDefinition()
    {
        CreateAThing<PowerUpDefinition>();
    }

    [MenuItem("Assets/Create/Power Up/Power Up Factory")]
    public static void CreatePowerUpFactory()
    {
        CreateAThing<PowerUpFactory>();
    }

    [MenuItem("Assets/Create/Power Up/Power Up Scan Map")]
    public static void CreatePowerUpScanMap()
    {
        CreateAThing<PowerUpScanMap>();
    }

    [MenuItem("Assets/Create/Animation/Animation Definition")]
    public static void CreateAnimationDefinition()
    {
        CreateAThing<AnimationDefinition>();
    }

    [MenuItem("Assets/Create/Animation/Animation Factory")]
    public static void CreateAnimationFactory()
    {
        CreateAThing<AnimationFactory>();
    }

    [MenuItem("Assets/Create/Popup/Popup Definition")]
    public static void CreatePopupDefinition()
    {
        CreateAThing<PopupDefinition>();
    }

    [MenuItem("Assets/Create/Popup/Popup Factory")]
    public static void CreatePopupFactory()
    {
        CreateAThing<PopupFactory>();
    }

    [MenuItem("Assets/Create/MapChunk/MapChunk Definition")]
    public static void CreateMapChunkDefinition()
    {
        CreateAThing<MapChunkDefinition>();
    }

    [MenuItem("Assets/Create/MapChunk/MapChunk Factory")]
    public static void CreateMapChunkFactory()
    {
        CreateAThing<MapChunkFactory>();
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

        var targetPath = string.Format("{0}/New {1}.asset", path, typeof(T));
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(targetPath);

        AssetDatabase.CreateAsset(instance, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = instance;
    }
}
