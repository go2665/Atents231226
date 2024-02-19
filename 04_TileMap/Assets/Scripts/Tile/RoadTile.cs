using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoadTile : Tile
{
    [Flags]                     // 이 enum은 bit flag로 사용한다고 표시하는 어트리뷰트
    enum AdjTilePosition : byte // 이 enum의 크기는 1byte
    {
        None = 0,       // 0000 0000
        North = 1,      // 0000 0001
        East = 2,       // 0000 0010
        South = 4,      // 0000 0100
        West = 8,       // 0000 1000
        All = North | East | South | West   // 0000 1111
    }

    /// <summary>
    /// 길을 구성할 스프라이트들
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// 타일이 그려질 때 자동으로 호출이 되는 함수
    /// </summary>
    /// <param name="position">타일의 위치(그리드 좌표)</param>
    /// <param name="tilemap">이 타일이 그려지는 타일맵</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // 주변에 있는 같은 종류의 타일 갱신하기
        for(int y = -1; y < 2; y++)
        {
            for(int x = -1; x < 2; x++)
            {
                Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);   // 주변 위치 결정
                if(HasThisTile(tilemap, location))      // 같은 타일인지 확인
                {
                    tilemap.RefreshTile(location);      // 같은 타일이면 갱신
                }
            }
        }
    }

    /// <summary>
    /// 타일맵의 RefreshTile함수가 호출될 때 호출, 타일이 어떤 스프라이트를 그릴지 결정하는 함수
    /// </summary>
    /// <param name="position">타일 데이터를 가져올 타일의 위치</param>
    /// <param name="tilemap">타일 데이터를 가져올 타일맵</param>
    /// <param name="tileData">가져온 타일 데이터의 참조(읽기 쓰기 둘 다 가능)</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // 4방향 확인. 확인한 내용을 저장
        AdjTilePosition mask = AdjTilePosition.None;

        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        //if(HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)))  // 윗줄과 똑같은 코드
        //{
        //    mask = mask | AdjTilePosition.North;
        //}
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.West : 0;

        // 이미지 선택하기
        int index = GetIndex(mask);
        if( index > -1 && index < sprites.Length )  // 인덱스가 제대로 골라졌는지 확인
        {
            tileData.sprite = sprites[index];           // 스프라이트 설정
            Matrix4x4 matrix = tileData.transform;
            matrix.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);    // 타일 회전 시키기
            tileData.transform = matrix;
            tileData.flags = TileFlags.LockTransform;   // 다른 타일이 회전을 못시키게 잠그기
        }
        else
        {
            Debug.LogError($"잘못된 인덱스 : {index}, mask = {mask}");
        }
    }

    /// <summary>
    /// 특정 타일맵의 특정 위치에 이 타일과 같은 종류의 타일이 있는지 확인하는 함수
    /// </summary>
    /// <param name="tilemap">확인할 타일맵</param>
    /// <param name="position">확인할 위치</param>
    /// <returns>true면 같은 종류의 타일, false면 다른 종류의 타일</returns>
    bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;   // 같은 종류인지 확인
    }

    /// <summary>
    /// 마스크 값에 따라 그릴 스프라이트의 인덱스를 리턴하는 함수
    /// </summary>
    /// <param name="mask">주변 타일의 상황을 표시한 비트플래그값</param>
    /// <returns>그려야할 스프라이트의 인덱스</returns>
    int GetIndex(AdjTilePosition mask)
    {
        int index = -1;

        switch(mask)
        {
            case AdjTilePosition.None:  // 없음
            case AdjTilePosition.North: // 북
            case AdjTilePosition.East:  // 동
            case AdjTilePosition.South: // 남
            case AdjTilePosition.West:  // 서
            case AdjTilePosition.North | AdjTilePosition.South: // 북남
            case AdjTilePosition.East | AdjTilePosition.West:   // 동서
                index = 0;// l자 모양의 스프라이트
                break;
            case AdjTilePosition.South | AdjTilePosition.West:
            case AdjTilePosition.West | AdjTilePosition.North:
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.East | AdjTilePosition.South:
                index = 1;// ㄱ자 모양의 스프라이트
                break;
            case AdjTilePosition.All & ~AdjTilePosition.North:    // (0000 1111) & ~(0000 0001) = 0000 1110 // 북쪽만 빼고
            case AdjTilePosition.All & ~AdjTilePosition.East:     // (0000 1111) & ~(0000 0010) = 0000 1101 // 동쪽만 빼고
            case AdjTilePosition.All & ~AdjTilePosition.South:    // (0000 1111) & ~(0000 0100) = 0000 1011 // 남쪽만 빼고
            case AdjTilePosition.All & ~AdjTilePosition.West:     // (0000 1111) & ~(0000 1000) = 0000 0111 // 서쪽만 빼고
                                                                  // ~notrh : ~(0000 0001) = 1111 1110
                index = 2;// ㅗ자 모양의 스프라이트
                break;
            case AdjTilePosition.All:
                index = 3;// +자 모양의 스프라이트
                break;
        }

        return index;
    }

    /// <summary>
    /// 마스크 값에 따라 스프라이트를 얼마나 회전시킬 것인지 결정하는 함수
    /// </summary>
    /// <param name="mask">주변 타일 상황을 기록해 놓은 마스크</param>
    /// <returns>최종 회전</returns>
    Quaternion GetRotation(AdjTilePosition mask)
    {
        Quaternion rotate = Quaternion.identity;
        switch(mask)
        {
            case AdjTilePosition.East:
            case AdjTilePosition.West:
            case AdjTilePosition.East | AdjTilePosition.West:   // ㅣ자
            case AdjTilePosition.North | AdjTilePosition.West:  // ㄱ자
            case AdjTilePosition.All & ~AdjTilePosition.West:   // ㅗ자
                // -90도 돌리기
                rotate = Quaternion.Euler(0, 0, -90);
                break;
            case AdjTilePosition.North | AdjTilePosition.East:  // ㄱ자
            case AdjTilePosition.All & ~AdjTilePosition.North:  // ㅗ자
                // -180도 돌리기
                rotate = Quaternion.Euler(0, 0, -180);
                break;
            case AdjTilePosition.South | AdjTilePosition.East:  // ㄱ자
            case AdjTilePosition.All & ~AdjTilePosition.East:  // ㅗ자
                // -270도 돌리기
                rotate = Quaternion.Euler(0, 0, -270);
                break;
        }
        return rotate;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Tiles/Custom/RoadTile")]    
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject( // 파일 저장창을 열고 입력결과를 돌려주는 함수
            "Sace Road Tile",   // 제목
            "New Road Tile",    // 디폴트 파일명
            "Asset",            // 파일의 디폴트 확장자
            "Save Road Tile",   // 출력 메세지
            "Assets/Tiles");    // 열릴 기본 폴더
        if(path != null)
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    // RoadTile를 파일로 저장
        }
    }
#endif
}
