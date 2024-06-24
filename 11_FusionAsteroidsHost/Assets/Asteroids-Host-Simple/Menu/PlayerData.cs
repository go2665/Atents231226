using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.HostSimple
{
    // This class functions as an Instance Singleton (no-static references)
    // and holds information about the local player in-between scene loads.

    // 아주 간단한 싱글톤으로 사용(스테틱 레퍼런스 없음)
    // 로컬 플레이어의 정보를 씬 전환할 때 유지하는 용도
    public class PlayerData : MonoBehaviour
    {
        private string _nickName = null;

        private void Start()
        {
            var count = FindObjectsOfType<PlayerData>().Length;     // 간단한 싱글톤 구현
            if (count > 1)
            {
                Destroy(gameObject);        // 이전에 만들어진 것이 있으면 자신을 삭제
                return;
            }

            DontDestroyOnLoad(gameObject);  // 처음 만든 것은 씬 전환시 삭제되지 않게 설정
        }

        // 이름 저장하기
        public void SetNickName(string nickName)
        {
            _nickName = nickName;
        }

        // 이름 가져오기
        public string GetNickName()
        {
            if (string.IsNullOrWhiteSpace(_nickName))   // 비어있으면
            {
                _nickName = GetRandomNickName();        // 랜덤한 이름으로 설정
            }

            return _nickName;   // 이름 리턴
        }

        // 랜덤한 이름을 리턴하는 함수(스테틱), 이름 겹칠 수 있음
        public static string GetRandomNickName()
        {
            var rngPlayerNumber = Random.Range(0, 9999);
            return $"Player {rngPlayerNumber.ToString("0000")}";    // 0000 ~ 9998까지 랜덤한 번호로 이름 만들기
        }
    }
}