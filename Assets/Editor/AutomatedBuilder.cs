using System;
using System.Linq;
using UnityEditor;

public static class AutomatedBuilder
{
    static void BuildWindowsDesktop()
    {
        Build(BuildTarget.StandaloneWindows64);
    }

    static void BuildMacDesktop()
    {
        Build(BuildTarget.StandaloneOSXUniversal);
    }

    static void BuildiOS()
    {
        Build(BuildTarget.iOS);
    }

    static void BuildAndroid()
    {
        Build(BuildTarget.Android);
    }

    private static void Build(BuildTarget target)
    {
        BuildPipeline.BuildPlayer(GetScenePaths(), Environment.GetEnvironmentVariable("OUTPUT_PATH"), target, BuildOptions.None);
    }

    private static string[] GetScenePaths()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }
}