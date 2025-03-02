using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor tool for creating BoidSettings ScriptableObjects.
/// Place this script in an Editor folder.
/// </summary>
#if UNITY_EDITOR
public class BoidScriptableObjectFactory
{
    [MenuItem("Assets/Create/Boid System/Boid Settings")]
    public static void CreateBoidSettings()
    {
        BoidSettings asset = ScriptableObject.CreateInstance<BoidSettings>();
        
        AssetDatabase.CreateAsset(asset, "Assets/BoidSettings.asset");
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
#endif