using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    /// <summary>
    /// 맵의 세로 개수
    /// </summary>
    const int HeightCount = 3;

    /// <summary>
    /// 맵의 가로 개수
    /// </summary>
    const int WidthCount = 3;

    /// <summary>
    /// 맵 하나의 세로 길이
    /// </summary>
    const float mapHeightSize = 20.0f;

    /// <summary>
    /// 맵 하나의 가로 길이
    /// </summary>
    const float mapWidthSize = 20.0f;

    /// <summary>
    /// 월드의 원점(모든 맵을 합쳤을 때 왼쪽 아래 구석이 원점)
    /// </summary>
    readonly Vector2 worldOrigin = new Vector2(-mapWidthSize * WidthCount * 0.5f, -mapHeightSize * HeightCount * 0.5f);

    /// <summary>
    /// 씬 이름 조합용 기본 이름
    /// </summary>
    const string SceneNameBase = "Seemless";

    /// <summary>
    /// 모든 씬의 이름을 저장한 배열
    /// </summary>
    string[] sceneNames;

    /// <summary>
    /// 씬의 로딩 상태를 나타낼 enum
    /// </summary>
    enum SceneLoadState : byte
    {
        Unload = 0,     // 로딩이 안되어있는 상태(해제되어 있는 상태)
        PendingUnload,  // 로딩 해제 진행중인 상태
        PendingLoad,    // 로딩 진행중인 상태
        Loaded          // 로딩 완료된 상태
    }

    /// <summary>
    /// 모든 씬의 로딩 상태
    /// </summary>
    SceneLoadState[] sceneLoadState;

    /// <summary>
    /// 모든 씬이 언로드 되었음을 확인하기 위한 프로퍼티(모든 씬이 Unload상태면 true, 아니면 false)
    /// </summary>
    public bool IsUnloadAll
    {
        get
        {
            bool result = true;
            foreach (SceneLoadState state in sceneLoadState)
            {
                if (state != SceneLoadState.Unload)  // 하나라도 unload가 아니면 false
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 로딩 요청이 들어온 씬의 목록
    /// </summary>
    List<int> loadWork = new List<int>();

    /// <summary>
    /// 로딩이 완료된 씬의 목록
    /// </summary>
    List<int> loadWorkComplete = new List<int>();

    /// <summary>
    /// 로딩 해제 요청이 들어온 씬의 목록
    /// </summary>
    List<int> unloadWork = new List<int>();

    /// <summary>
    /// 로딩 해제가 완료된 씬의 목록
    /// </summary>
    List<int> unloadWorkComplete = new List<int>();

    /// <summary>
    /// 처음 만들어졌을 때 한번만 실행되는 함수
    /// </summary>
    public void PreInitialize()
    {
        int mapCount = HeightCount * WidthCount;
        sceneNames = new string[mapCount];
        sceneLoadState = new SceneLoadState[mapCount];

        for (int y = 0; y < HeightCount; y++)
        {
            for (int x = 0; x < WidthCount; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{SceneNameBase}_{x}_{y}";
                sceneLoadState[index] = SceneLoadState.Unload;
            }
        }
    }

    /// <summary>
    /// 씬이 single로 로드될 때마다 호출될 초기화용 함수
    /// </summary>
    public void Initialize()
    {
        // 맵 로딩 상태 초기화
        for (int i = 0; i < sceneLoadState.Length; i++)
        {
            sceneLoadState[i] = SceneLoadState.Unload;
        }

        // 플레이어 관련 처리
        Player player = GameManager.Instance.Player;
        if(player != null)
        {
            player.onMapChange += (currentGrid) =>
            {
                // 맵에서 위치가 바뀌면 주변맵 갱신하기
                RefreshScenes(currentGrid.x, currentGrid.y);
            };

            player.onDie += (_, _) =>
            {
                // 플레이어가 죽으면 모든 맵을 로딩해제 요청하기
                for(int y= 0; y < HeightCount;y++)
                {
                    for(int x = 0; x < WidthCount;x++)
                    {
                        RequestAsyncSceneUnload(x, y);                        
                    }
                }
            };

            Vector2Int grid = WorldToGrid(player.transform.position);   // 플레이어가 있는 맵의 그리드값 가져오기
            RequestAsyncSceneLoad(grid.x, grid.y);  // 플레이어가 있는 맵을 최우선으로 로딩 요청
            RefreshScenes(grid.x, grid.y);          // 주변맵 로딩 요청
        }
    }

    /// <summary>
    /// 맵의 그리드 위치를 인덱스로 변경해주는 함수
    /// </summary>
    /// <param name="x">맵의 x위치</param>
    /// <param name="y">맵의 y위치</param>
    /// <returns>배열용 인덱스값</returns>
    int GetIndex(int x, int y)
    {
        return x + y * WidthCount;
    }

    /// <summary>
    /// 비동기 로딩 요청 함수
    /// </summary>
    /// <param name="x">로딩할 맵의 x위치</param>
    /// <param name="y">로딩할 맵의 y위치</param>
    void RequestAsyncSceneLoad(int x, int y)
    {
        int index = GetIndex(x, y);     // 인덱스 계산
        if (sceneLoadState[index] == SceneLoadState.Unload)
        {
            loadWork.Add(index);        // Unload인 씬만 로드 리스트에 추가
        }
    }

    /// <summary>
    /// 씬을 비동기로 로딩하는 함수(Additive 로딩)
    /// </summary>
    /// <param name="index">로딩할 맵의 인덱스</param>
    void AsyncSceneLoad(int index)
    {
        if (sceneLoadState[index] == SceneLoadState.Unload) // Unload 상태인 맵만 처리
        {
            sceneLoadState[index] = SceneLoadState.PendingLoad; // panding 상태로 만들어서 진행 중이라고 표시

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);  // 비동기 로딩 시작
            async.completed += (_) =>   // 비동기 작업이 끝났을 때 실행되는 델리게이트에 람다함수 추가
            {
                sceneLoadState[index] = SceneLoadState.Loaded;  // Loaded 상태로 변경
                loadWorkComplete.Add(index);                    // 완료 목록에 추가
            };
        }
    }

    /// <summary>
    /// 비동기 로딩 해제 요청 함수
    /// </summary>
    /// <param name="x">로딩해제할 맵의 x위치</param>
    /// <param name="y">로딩해제할 맵의 y위치</param>
    void RequestAsyncSceneUnload(int x, int y)
    {
        int index = GetIndex(x, y);     // 인덱스 계산
        if (sceneLoadState[index] == SceneLoadState.Loaded)
        {
            unloadWork.Add(index);      // 로딩 완료되었는 씬만 unload 리스트에 추가
        }
    }

    /// <summary>
    /// 비동기 로딩 해제를 처리하는 함수
    /// </summary>
    /// <param name="index">로딩 해제할 맵의 인덱스</param>
    void AsyncSceneUnload(int index)
    {
        if (sceneLoadState[index] == SceneLoadState.Loaded)         // 로딩이 완료된 맵만 처리
        {
            sceneLoadState[index] = SceneLoadState.PendingUnload;   // 로딩 해제중이라고 표시

            // 맵에 있는 슬라임을 풀로 되돌리기(씬 언로드로 삭제되는 것 방지)
            Scene scene = SceneManager.GetSceneByName(sceneNames[index]);   // 씬 찾기
            if (scene.isLoaded) // 씬이 로딩 되어 있으면
            {
                GameObject[] sceneRoots = scene.GetRootGameObjects();       // 루트 오브젝트 모두 찾기
                if (sceneRoots != null && sceneRoots.Length > 0)            // 루트 오브젝트가 1개 이상 있으면
                {
                    Slime[] slimes = sceneRoots[0].GetComponentsInChildren<Slime>();    // 슬라임 모두 찾아서
                    foreach (Slime slime in slimes)
                    {
                        slime.ReturnToPool();                               // 풀로 되돌리기
                    }
                }
            }

            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]);    // 비동기 로딩 해제 시작
            async.completed += (_) =>
            {
                sceneLoadState[index] = SceneLoadState.Unload;  // unload로 표시
                unloadWorkComplete.Add(index);                  // 완료 리스트에 추가
            };
        }
    }

    private void Update()
    {
        // 완료된 작업은 리스트에서 제거
        foreach (var index in loadWorkComplete)
        {
            loadWork.RemoveAll((x) => x == index);  // loadWork리스트에서 index와 같은 아이템은 모두 제거
        }
        loadWorkComplete.Clear();

        // 들어온 요청 처리
        foreach (var index in loadWork)
        {
            AsyncSceneLoad(index);  // 비동기 로딩 시작
        }

        // 완료된 작업은 리스트에서 제거
        foreach (var index in unloadWorkComplete)
        {
            unloadWork.RemoveAll((x) => x == index);  // unloadWork리스트에서 index와 같은 아이템은 모두 제거
        }
        unloadWorkComplete.Clear();

        // 들어온 요청 처리
        foreach (var index in unloadWork)
        {
            AsyncSceneUnload(index);    // 비동기 로딩 해제 시작(슬라임 되돌리기)
        }
    }

    /// <summary>
    /// 지정된 위치 주변 맵은 로딩 요청하고 그 외는 로딩 해제를 요청하는 함수
    /// </summary>
    /// <param name="mapX">지정된 맵의 X 위치</param>
    /// <param name="mapY">지정된 맵의 Y 위치</param>
    void RefreshScenes(int mapX, int mapY)
    {
        int startX = Mathf.Max(0, mapX - 1);            // 최소점은 (mapX,mapY)보다 1작거나 (0,0)
        int startY = Mathf.Max(0, mapY - 1);
        int endX = Mathf.Min(WidthCount, mapX + 2);     // 최대점은 (mapX,mapY)보다 1크거나 (WidthCount,HeightCount)
        int endY = Mathf.Min(HeightCount, mapY + 2);

        // (mapX,mapY) 주변은 RequestAsyncSceneLoad 실행
        List<Vector2Int> open = new List<Vector2Int>(9);
        for (int y = startY; y < endY; y++)
        {
            for(int x = startX; x < endX; x++)
            {
                RequestAsyncSceneLoad(x, y);        // 해당 하는 것들 로딩 요청
                open.Add(new Vector2Int(x, y));     // 로딩 요청한 것들 기록
            }
        }

        // 나머지 부분은 모두 RequestAsyncSceneUnload 실행
        for (int y = 0; y<HeightCount; y++)
        {
            for(int x = 0; x < WidthCount; x++)
            {
                // Contains : 단순 있다/없다 확인용
                // Exits : 특정 조건을 만족하는 것이 있는지 없는지 확인용
                if( !open.Contains(new Vector2Int(x,y)))
                {
                    RequestAsyncSceneUnload(x, y);                    
                }
            }
        }        
    }

    /// <summary>
    /// 월드 좌표가 어떤 맵에 속하는지 계산하는 함수
    /// </summary>
    /// <param name="worldPostion">확인할 월드 좌표</param>
    /// <returns>맵의 좌표( (0,0) ~ (2,2) )</returns>
    public Vector2Int WorldToGrid(Vector3 worldPostion)
    {
        Vector2 offset = (Vector2)worldPostion - worldOrigin;

        return new Vector2Int((int)(offset.x/mapWidthSize), (int)(offset.y/mapHeightSize));
    }

#if UNITY_EDITOR
    public void TestLoadScene(int x, int y)  
    { 
        RequestAsyncSceneLoad(x, y);
    }

    public void TestUnloadScene(int x, int y)
    {
        RequestAsyncSceneUnload(x, y);
    }

    public void TestRefreshscenes(int x, int y)
    {
        RefreshScenes(x, y);
    }

    public void TestUnloadAllScene()
    {
        for(int y = 0;y<HeightCount;y++)
            for(int x = 0;x < WidthCount;x++)
                RequestAsyncSceneUnload(x, y);
    }
#endif
}
