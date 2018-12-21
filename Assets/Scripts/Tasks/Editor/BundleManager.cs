using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BundleManager : EditorWindow
{

    private BuildTarget BuildTarget = BuildTarget.StandaloneWindows;
    private BuildAssetBundleOptions BundleOptions = BuildAssetBundleOptions.None;

    private string OutPath;
    private string AssetBundleName;

    [MenuItem("Window/Bundle Manager")]
    private static void OpenWindow()
    {
        GetWindow<BundleManager>("Bundle Manager");
        
    }

    private void OnGUI()
    {
        AssetBundleName = EditorGUILayout.TextField("Bundle Name", AssetBundleName);
        BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Target Platform", BuildTarget);
        BundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup("Bundle options", BundleOptions);
        OutPath = EditorGUILayout.TextField("Out Path", OutPath);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        string[] assetNames;
        if (GUILayout.Button("Open", GUILayout.ExpandWidth(false)))
        {
            OutPath = EditorUtility.OpenFolderPanel("Open", "", "");
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Build selected"))
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            assetNames = new string[selectedObjects.Length];
            for (int i = 0; i < assetNames.Length; i++)
            {
                assetNames[i] = AssetDatabase.GetAssetPath(selectedObjects[i]);
                Debug.Log(assetNames[i]);
                AssetBundleBuild[] build = new AssetBundleBuild[1];
                build[0] = new AssetBundleBuild();
                build[0].assetBundleName = AssetBundleName;
                build[0].assetNames = assetNames;
                BuildPipeline.BuildAssetBundles(OutPath, build, BundleOptions, BuildTarget);
            }
        }

    }
    
    
    
}
