using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildAndroid {
    public static void Build() {
        Directory.CreateDirectory("build/Android");
        PlayerSettings.productName = "CELL DEFENDER";
        PlayerSettings.companyName = "SunTung";
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.suntung.celldefender");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        var scenes = new[] { "Assets/Scenes/Main.unity" };
        var options = new BuildPlayerOptions {
            scenes = scenes,
            locationPathName = "build/Android/CELL-DEFENDER.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Android build failed: " + report.summary.result);
    }
}
