using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int enemyCount = 50;
    public GameObject enemyPrefab;

    int mazeWidth;
    int mazeHeight;
    Player player;

    private void Start()
    {
        // 미로 크기 가져오기
        mazeWidth = GameManager.Instance.MazeWidth;
        mazeHeight = GameManager.Instance.MazeHeight;

        player = GameManager.Instance.Player;

        // 적 생성
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, transform);
            obj.name = $"Enemy_{i}";
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.onDie += (target) =>
            {
                StartCoroutine(Respawn(target));
            };
            enemy.Respawn(GetRandomSpawnPosition(true));
        }
    }

    /// <summary>
    /// 플레이어 주변의 랜덤한 스폰 위치를 구하는 함수
    /// </summary>
    /// <returns>스폰 위치(미로 한 셀의 가운데 지점)</returns>
    Vector3 GetRandomSpawnPosition(bool init = false)
    {
        Vector2Int playerPostion;   // 플레이어의 그리드 위치

        if(init)
        {
            // 플레이어가 정상적으로 있다는 보장이 없는 경우 그냥 미로의 가운데 위치
            playerPostion = new(mazeWidth / 2, mazeHeight / 2); 
        }
        else
        {
            // 일반 플레이 중에는 플레이어의 그리드 위치
            playerPostion = MazeVisualizer.WorldToGrid(player.transform.position);
        }

        int x;
        int y;
        do
        {
            // 플레이어 위치에서  +-5 범위 안이 걸릴 때까지 랜덤돌리기
            int index = Random.Range(0, mazeHeight * mazeWidth);
            x = index / mazeWidth;
            y = index % mazeHeight;
        }while( x < playerPostion.x + 5 && x > playerPostion.x - 5 && y < playerPostion.y + 5 && y > playerPostion.y - 5);

        Vector3 world = MazeVisualizer.GridToWorld(x, y);

        return world;
    }

    /// <summary>
    /// 일정 시간 후에 target을 리스폰 시키는 코루틴
    /// </summary>
    /// <param name="target">리스폰 대상</param>
    /// <returns></returns>
    IEnumerator Respawn(Enemy target)
    {
        yield return new WaitForSeconds(3);
        target.Respawn(GetRandomSpawnPosition());
    }

}
