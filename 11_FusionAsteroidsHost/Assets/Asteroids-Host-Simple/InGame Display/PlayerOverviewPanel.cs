using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Asteroids.HostSimple
{
    // 플레이어의 정보를 표시하는 것을 관리하는 순수 유틸리티 클래스
    // 모든 메서드는 PlayerDataNetworked의 정보가 변경됨을 감지하면 호출된다.
    public class PlayerOverviewPanel : MonoBehaviour
    {
        // 한 플레이어의 정보를 표시하는 UI의 프리팹
        [SerializeField] private TextMeshProUGUI _playerOverviewEntryPrefab = null;

        // 전체 플레이어의 정보 표시용 UI를 가지는 딕셔너리
        private Dictionary<PlayerRef, TextMeshProUGUI>
            _playerListEntries = new Dictionary<PlayerRef, TextMeshProUGUI>();

        // 각 플레이어가 표시할 정보
        private Dictionary<PlayerRef, string> _playerNickNames = new Dictionary<PlayerRef, string>();
        private Dictionary<PlayerRef, int> _playerScores = new Dictionary<PlayerRef, int>();
        private Dictionary<PlayerRef, int> _playerLives = new Dictionary<PlayerRef, int>();

        // 새 엔트리 생성
        public void AddEntry(PlayerRef playerRef, PlayerDataNetworked playerDataNetworked)
        {
            if (_playerListEntries.ContainsKey(playerRef)) return;  // 이미 있는 사람을 추가하려고 하면 스킵
            if (playerDataNetworked == null) return;                // playerDataNetworked가 null이면 스킵

            var entry = Instantiate(_playerOverviewEntryPrefab, this.transform);    // 새 엔트리 생성
            entry.transform.localScale = Vector3.one;       // 스케일 초기화???(이유를 알 수 없음)
            entry.color = SpaceshipVisualController.GetColor(playerRef.PlayerId);   // 엔트리 표시 텍스트 색상 설정

            string nickName = String.Empty; // 초기값으로 설정
            int lives = 0;
            int score = 0;

            _playerNickNames.Add(playerRef, nickName);
            _playerScores.Add(playerRef, score);
            _playerLives.Add(playerRef, lives);

            _playerListEntries.Add(playerRef, entry);   // 초기값으로 우선 딕셔너리에 항목만들기

            UpdateEntry(playerRef, entry);              // 일단 업데이트
        }

        // 엔트리에서 제거
        public void RemoveEntry(PlayerRef playerRef)
        {
            if (_playerListEntries.TryGetValue(playerRef, out var entry) == false) return;  // 딕셔너리에 없으면 스킵

            if (entry != null)
            {
                Destroy(entry.gameObject);  // 있는 것 제거
            }

            // 딕셔너리들에서도 해당 playerRef에 해당하는 항목들 제거
            _playerNickNames.Remove(playerRef);     
            _playerScores.Remove(playerRef);
            _playerLives.Remove(playerRef);

            _playerListEntries.Remove(playerRef);
        }

        // 생명 업데이트 함수(처음 스폰되었을 때, 변경 감지되었을 때 호출)
        public void UpdateLives(PlayerRef player, int lives)
        {
            if (_playerListEntries.TryGetValue(player, out var entry) == false) return;

            _playerLives[player] = lives;
            UpdateEntry(player, entry);
        }

        // 점수 업데이트 함수(처음 스폰되었을 때, 변경 감지되었을 때 호출)
        public void UpdateScore(PlayerRef player, int score)
        {
            if (_playerListEntries.TryGetValue(player, out var entry) == false) return;

            _playerScores[player] = score;
            UpdateEntry(player, entry);
        }

        // 이름 업데이트 함수(처음 스폰되었을 때, 변경 감지되었을 때 호출)
        public void UpdateNickName(PlayerRef player, string nickName)
        {
            if (_playerListEntries.TryGetValue(player, out var entry) == false) return; // 엔트리에 들어있는 플레이어인지 확인

            _playerNickNames[player] = nickName;    // 값 변경하고
            UpdateEntry(player, entry);             // 표시 업데이트 요청
        }

        // 각 플레이어 별로 UI를 갱신하는 함수(엔트리 추가되었을 때, 각 정보 업데이트 함수에서 호출)
        private void UpdateEntry(PlayerRef player, TextMeshProUGUI entry)
        {
            var nickName = _playerNickNames[player];    // 데이터 가져오고
            var score = _playerScores[player];
            var lives = _playerLives[player];

            entry.text = $"{nickName}\nScore: {score}\nLives: {lives}"; // 문자열 조합해서 표시
        }
    }
}