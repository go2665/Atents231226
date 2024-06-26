using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Asteroids.HostSimple
{
    // 우주선의 비주얼적인 면을 컨트롤하는 클래스(엔진 불길과 파괴용 VFX도 제공)
    public class SpaceshipVisualController : MonoBehaviour
    {
        // 배의 3D 모델을 그리는 메시 랜더러
        [SerializeField] private MeshRenderer _spaceshipModel = null;

        // 배가 터지는 이팩트
        [SerializeField] private ParticleSystem _destructionVFX = null;

        // 엔진 트레일 이팩트
        [SerializeField] private ParticleSystem _engineTrailVFX = null;

        // PlayerRef를 이용해서 배의 색상을 지정
        public void SetColorFromPlayerID(int playerID)
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.material.color = GetColor(playerID);  // 모든 랜더러의 머티리얼의 색상을 플레이어ID와 연관된 색으로 지정
            }
        }

        // 스폰 되었을 때 실행(죽었다 리스폰 되었을 때 포함)
        public void TriggerSpawn()
        {
            _spaceshipModel.enabled = true; // 모델 보여주기
            _engineTrailVFX.Play();         // 엔진 트레일 켜기
            _destructionVFX.Stop();         // 파괴 이팩트 끄기
        }

        // 파괴 되었을 때 실행
        public void TriggerDestruction()
        {
            _spaceshipModel.enabled = false;    // 모델 안보여 주기
            _engineTrailVFX.Stop();             // 엔진트레일 끄기
            _destructionVFX.Play();             // 파괴 이팩트 켜기
        }

        // 플레이어를 구별하기 위한 색상셋을 정의(기본적으로 최대 4인 플레이만 지원. 8개 색상을 한 이유는 최대한 안겹치기 위해)
        public static Color GetColor(int player)
        {
            switch (player%8)
            {
                case 0: return Color.red;
                case 1: return Color.green;
                case 2: return Color.blue;
                case 3: return Color.yellow;
                case 4: return Color.cyan;
                case 5: return Color.grey;
                case 6: return Color.magenta;
                case 7: return Color.white;
            }
            return Color.black;
        }
    }
}