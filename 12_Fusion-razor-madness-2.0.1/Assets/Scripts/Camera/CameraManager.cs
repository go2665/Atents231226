using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 카메라 이동 속도
    public float Speed = 1f;
    // 카메라가 따라다닐 대상
    public Transform CameraTarget;

    // 카메라와 CameraTarget의 위치 간격
    private Vector3 _offset = new Vector3(0, 5, 0);
    // 카메라가 한 프레임에 움직이는 거리
    private float _step;

    // 관전 대상 목록(본인 포함)
    private List<PlayerBehaviour> _spectatingList = new List<PlayerBehaviour>();
    // 관전모드인지 아닌지 확인하는 변수
    private bool _spectating = false;

    /// <summary>
    /// 관전모드로 변경하는 함수
    /// </summary>
    public void SetSpectating()
    {
        _spectating = true;
        _spectatingList = new List<PlayerBehaviour>(FindObjectsOfType<PlayerBehaviour>());  // 모든 플레이어에 대한 리스트 만들기
        CameraTarget = GetRandomSpectatingTarget(); // 새 CameraTarget 구하기
    }

    /// <summary>
    /// 자신이 아닌 관전 대상을 CameraTarget으로 설정하기(1명만 있을 떄는 자기 자신)
    /// </summary>
    /// <returns></returns>
    private Transform GetRandomSpectatingTarget()
    {
        if (_spectatingList.Count == 1)
        {
            return CameraTarget;    // 한명만 있을 때는 자기 자신
        }
        return _spectatingList.Find(x => x.transform.GetChild(0) != CameraTarget).transform.GetChild(0);    // 내가 아닌 첫번째 고르기
    }

    /// <summary>
    /// 다음 플레이어나 이전 플레이어의 CameraTarget 구하기
    /// </summary>
    /// <param name="to">+1이면 다음 캐릭터, -1이면 이전 캐릭터</param>
    /// <returns></returns>
    private Transform GetNextOrPrevSpectatingTarget(int to)
    {
        // 현재 선택된 타겟의 인덱스 구하기(리스트에서의 인덱스)
        int currentIndex = _spectatingList.IndexOf(CameraTarget.GetComponentInParent<PlayerBehaviour>());

        if (currentIndex + to >= _spectatingList.Count)
        {
            return _spectatingList[0].transform.GetChild(0);    // 더했을 때 최대 개수를 넘기면 앞으로 돌아가기
        }
        else if (currentIndex + to < 0)
        {
            return _spectatingList[_spectatingList.Count - 1].transform.GetChild(0);    // 0미만이 되면 마지막으로 돌리기
        }
        else
        {
            return _spectatingList[currentIndex+to].transform.GetChild(0);  // 일반적인 경우
        }
    }

    private void Update()
    {
        if (_spectating)    // 관전 모드일 경우
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CameraTarget = GetNextOrPrevSpectatingTarget(1);    // 오른쪽 화살표를 누르면 다음 캐릭터를 관전
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CameraTarget = GetNextOrPrevSpectatingTarget(-1);   // 왼쪽 화살표를 누르면 이전 캐릭터를 관전
            }
        }
    }

    private void LateUpdate()
    {
        if (CameraTarget == null)   // 타겟이 없으면 처리 안함
        {
            return;
        }

        // Lerp함수를 풀어서 써 놓은 코드
        _step = Speed * Vector2.Distance(CameraTarget.position, transform.position) * Time.deltaTime;   // 한번에 이동할 양 결정
        Vector2 pos = Vector2.MoveTowards(transform.position, CameraTarget.position + _offset, _step);  // 위에서 계산된만큼 움직이기
        transform.position = pos;
    }
}
