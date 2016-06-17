using System;
using System.Linq;
using UnityEditor;

public static class AutomatedBuilder
{
    private const string OUTPUT_PATH_COMMAND_LINE_ARGUMENT_NAME = "-outputPath";

    static void BuildWindowsDesktop()
    {
        Build("Builds/Windows/App.exe", BuildTarget.StandaloneWindows64);
    }

    static void BuildMacDesktop()
    {
        Build("Builds/OSX/App.app", BuildTarget.StandaloneOSXUniversal);
    }

    static void BuildiOS()
    {
        Build("Builds/iOS/App", BuildTarget.iOS);
    }

    static void BuildAndroid()
    {
        Build("Builds/Android/App.apk", BuildTarget.Android);
    }

    private static void Build(string defaultPath, BuildTarget target)
    {
        var path = defaultPath;
        var arguments = Environment.GetCommandLineArgs();

        for (var i = 0; i < arguments.Length; i++)
        {
            if ((arguments[i] == OUTPUT_PATH_COMMAND_LINE_ARGUMENT_NAME) && (i + 1 < arguments.Length))
            {
                path = arguments[i + 1];
                break;
            }
        }

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