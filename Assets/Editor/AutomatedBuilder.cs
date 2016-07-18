using System;
using System.Linq;
using UnityEditor;

public static class AutomatedBuilder
{
    private static string[] arguments = Environment.GetCommandLineArgs();

    private const string OUTPUT_PATH_ARGUMENT_NAME = "-outputPath";
    private const string BUNDLE_VERSION_ARGUMENT_NAME = "-version";
    private const string DEVELOPMENT_BUILD_ARGUMENT_NAME = "-developmentBuild";
    private const string ANDROID_KEYSTORE_PASS_ARGUMENT_NAME = "-keystorePass";
    private const string ANDROID_KEYALIAS_PASS_ARGUMENT_NAME = "-keyaliasPass";

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
        PlayerSettings.keystorePass = GetCommandLineArgument(ANDROID_KEYSTORE_PASS_ARGUMENT_NAME);
        PlayerSettings.keyaliasPass = GetCommandLineArgument(ANDROID_KEYALIAS_PASS_ARGUMENT_NAME);

        Build("Builds/Android/App.apk", BuildTarget.Android);
    }

    private static void Build(string defaultPath, BuildTarget target)
    {
        SetBundleVersion();

        var commandLinePath = GetCommandLineArgument(OUTPUT_PATH_ARGUMENT_NAME);
        var path = (commandLinePath != "") ? commandLinePath : defaultPath;
        Console.WriteLine("Output Path: " + path);

        BuildOptions options = BuildOptions.None;

        if (GetCommandLineArgument(DEVELOPMENT_BUILD_ARGUMENT_NAME) == "true")
        {
            Console.WriteLine("Development build options enabled");
            options |= BuildOptions.Development;
            options |= BuildOptions.AllowDebugging;
        }

        Console.WriteLine("Starting build...");
        BuildPipeline.BuildPlayer(GetScenePaths(), path, target, options);

        Console.WriteLine("Done");
    }

    private static string[] GetScenePaths()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }

    private static void SetBundleVersion()
    {
        var bundleVersion = GetCommandLineArgument(BUNDLE_VERSION_ARGUMENT_NAME);

        if (bundleVersion != "")
        {
            PlayerSettings.bundleVersion = bundleVersion;
        }

        Console.WriteLine("Bundle Version: " + bundleVersion);
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