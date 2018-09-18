using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
public static class BuilderExample
{
    [MenuItem("Tools/Build AssetBundles/Build Windows64")]
    static void BuildWindows64()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath, BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Tools/Build AssetBundles/Build Android")]
    static void BuildAndroid()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("Tools/Build AssetBundles/Build iOS")]
    static void BuildIOS()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath, BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
}