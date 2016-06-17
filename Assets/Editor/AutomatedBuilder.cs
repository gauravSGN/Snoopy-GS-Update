using System;
using System.Linq;
using UnityEditor;

public static class AutomatedBuilder
{
    private static string[] arguments = Environment.GetCommandLineArgs();

    private const string ANDROID_KEYSTORE_PASS = "-keystorePass";
    private const string ANDROID_KEYALIAS_PASS = "-keyaliasPass";
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
        PlayerSettings.keystorePass = GetCommandLineArgument(ANDROID_KEYSTORE_PASS);
        PlayerSettings.keyaliasPass = GetCommandLineArgument(ANDROID_KEYALIAS_PASS);

        Build("Builds/Android/App.apk", BuildTarget.Android);
    }

    private static void Build(string defaultPath, BuildTarget target)
    {
        var commandLinePath = GetCommandLineArgument(OUTPUT_PATH_COMMAND_LINE_ARGUMENT_NAME);
        var path = (commandLinePath != "") ? commandLinePath : defaultPath;
        Console.WriteLine("Output Path: " + path);

        Console.WriteLine("Starting build...");
        BuildPipeline.BuildPlayer(GetScenePaths(), path, target, BuildOptions.None);

        Console.WriteLine("Done");
    }

    private static string[] GetScenePaths()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }

    private static string GetCommandLineArgument(string argument)
    {
        string argumentValue = "";

        for (var index = 0; index < arguments.Length; index++)
        {
            if (arguments[index] == argument)
            {
                var nextIndex = index + 1;

                if ((nextIndex < arguments.Length) && (arguments[nextIndex][0] != '-'))
                {
                    argumentValue = arguments[nextIndex];
                    break;
                }
            }
        }

        return argumentValue;
    }
}