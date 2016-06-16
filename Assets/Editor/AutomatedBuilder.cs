using UnityEngine;
using UnityEditor;

public static class AutomatedBuilder
{
    [MenuItem("File/Build/iOS")]
    static void PerformiOSBuild()
    {
        PerformAutomatedbuild("Builds/iOS", BuildTarget.iOS);
    }

    [MenuItem("File/Build/Android")]
    static void PerformAndroidBuild()
    {
        PerformAutomatedbuild("Builds/Android", BuildTarget.Android);
    }

    [MenuItem("File/Build/OSX Desktop")]
    static void PerformDesktopBuild()
    {
        PerformAutomatedbuild("Builds/OSX Desktop", BuildTarget.StandaloneOSXUniversal);
    }

    private static void PerformAutomatedbuild(string path, BuildTarget target)
    {
        Debug.Log("Switching Build Target");
        EditorUserBuildSettings.SwitchActiveBuildTarget(target);

        Debug.Log("Building...");
        BuildPipeline.BuildPlayer(GetScenePaths(), path, target, BuildOptions.None);

        Debug.Log("Done");
    }

    private static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }
}