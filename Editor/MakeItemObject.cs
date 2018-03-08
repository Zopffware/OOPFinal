using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeItemObject {
    [MenuItem("Assets/Create/Item Object")]
    public static void Create()
    {
        ItemObject asset = ScriptableObject.CreateInstance<ItemObject>();
        AssetDatabase.CreateAsset(asset, "Assets/NewItemObject.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

}
