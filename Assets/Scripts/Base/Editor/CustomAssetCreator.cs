using UnityEditor;
using UnityEngine;
using System.IO;
using BubbleContent;
using PowerUps;
using Animation;

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

    [MenuItem("Assets/Create/Bubbles/Bubble Content Definition")]
    public static void CreateBubbleContentDefinition()
    {
        CreateAThing<BubbleContentDefinition>();
    }

    [MenuItem("Assets/Create/Bubbles/Bubble Content Factory")]
    public static void CreateBubbleContentFactory()
    {
        CreateAThing<BubbleContentFactory>();
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
