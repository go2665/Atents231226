using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int enemyCount = 50;
    public GameObject enemyPrefab;

    int mazeWidth;
    int mazeHeight;

    private void Start()
    {
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
            enemy.Respawn(GetRandomSpawnPosition());
        }

        // 미로 크기 가져오기
        mazeWidth = GameManager.Instance.MazeWidth;
        mazeHeight = GameManager.Instance.MazeHeight;
    }

    /// <summary>
    /// 플레이어 주변의 랜덤한 스폰 위치를 구하는 함수
    /// </summary>
    /// <returns>스폰 위치(미로 한 셀의 가운데 지점)</returns>
    Vector3 GetRandomSpawnPosition()
    {
        return Vector3.zero;
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
