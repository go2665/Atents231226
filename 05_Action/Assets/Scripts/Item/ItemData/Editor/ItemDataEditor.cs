using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ItemData), true)]
public class ItemDataEditor : Editor
{
    ItemData itemData;

    private void OnEnable()
    {
        itemData = target as ItemData;
    }

    public override void OnInspectorGUI()
    {
        if(itemData != null && itemData.itemIcon != null)
        {
            Texture2D texture;
            EditorGUILayout.LabelField("Item Icon Preview");
            texture = AssetPreview.GetAssetPreview(itemData.itemIcon);
            if(texture != null)
            {
                GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64));
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
        }

        base.OnInspectorGUI();
    }
}
#endif