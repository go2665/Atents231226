using UnityEngine;
using UnityEngine.EventSystems;

namespace Asteroids.HostSimple
{
    /// <summary>
    /// 이벤트 시스템 스포너
    /// EventSystem 게임 오브젝트 추가(EventSystem, StandaloneInputModule 컴포넌트 포함)
    /// Additive로 씬을 로딩할 때 유니티가 "EventSystem이 여러개인 씬은 지원하지 않습니다"라는 에러를 띄우면 사용해라
    /// </summary>
    public class EventSystemSpawner : MonoBehaviour
    {
        void OnEnable()
        {
            EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();
            if (sceneEventSystem == null)   // 찾아서 없을 때만 만든다.
            {
                GameObject eventSystem = new GameObject("EventSystem"); // 빈 게임 오브젝트를 "EventSystem"이라는 이름으로 생성

                eventSystem.AddComponent<EventSystem>();            // EventSystem 컴포넌트 추가
                eventSystem.AddComponent<StandaloneInputModule>();  // StandaloneInputModule 컴포넌트 추가
            }
        }
    }
}
