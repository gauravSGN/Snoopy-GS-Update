using System;
using System.Linq;
using UnityEngine;
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
        var path = Environment.GetEnvironmentVariable("OUTPUT_PATH");
        Console.WriteLine("Output Path: " + path);

        Console.WriteLine("Starting build...");
        BuildPipeline.BuildPlayer(GetScenePaths(), path, target, BuildOptions.None);
        Console.WriteLine("Done");
    }

    private static string[] GetScenePaths()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }
}