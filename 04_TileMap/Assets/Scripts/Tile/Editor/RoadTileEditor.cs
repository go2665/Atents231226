//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor;
//[CustomEditor(typeof(RoadTile))]        // RoadTile용 커스텀 데이터라고 표시하는 어트리뷰트.
//public class RoadTileEditor : Editor
//{
//    /// <summary>
//    /// 선택된 roadTile
//    /// </summary>
//    RoadTile roadTile;

//    private void OnEnable()
//    {
//        roadTile = target as RoadTile;  // target은 클릭된 에셋. target이 RoadTile인지 확인해서 캐스팅
//    }

//    /// <summary>
//    /// 인스펙터 창 내부를 그리는 함수
//    /// </summary>
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();  // 원래 그리던 대로 그리기

//        // 추가로 그리기
//        if(roadTile != null)    // roadTile이 있어야 함
//        {
//            if(roadTile.sprites != null)    // sprites도 있어야 함
//            {
//                EditorGUILayout.LabelField("Sprites Image Preview");    // 제목 적기

//                Texture2D texture;
//                for(int i = 0; i < roadTile.sprites.Length; i++)        // sprites에 있는 이미지를 하나씩 그리기
//                {
//                    texture = AssetPreview.GetAssetPreview(roadTile.sprites[i]);    // sprite를 texture로 변경
//                    if(texture != null)
//                    {
//                        GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // 64*64크기 잡기
//                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // 크기 잡은 곳에 텍스쳐 그리기
//                    }
//                }
//            }
//        }
//    }
//}

//#endif