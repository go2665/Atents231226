using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // NextTarget is called before the first frame update
    void Start()
    {
        GameObject obj1 = GameObject.Find("이름");  // 이름으로찾기
        GameObject obj2 = GameObject.FindWithTag("tag"); // 태그로찾기 - 함수 이름만 줄여 놓은 것
        GameObject obj3 = GameObject.FindGameObjectWithTag("tag"); //태그로찾기
        GameObject[] obj4 = GameObject.FindGameObjectsWithTag("tag");  //같은 태그 모두 찾기
        GameObject obj5 = GameObject.FindAnyObjectByType<GameObject>(); // 특정타입으로 찾기(아무거나 1개만. 뭐가 나올지 예상불가능, FindFirstObjectByType보다 빠르다.)
        GameObject obj6 = GameObject.FindFirstObjectByType<GameObject>();   // 특정타입으로 찾기(타입중 첫번째)

        // 특정타입 모두 찾기(파라메터로 비활성화된 오브젝트 포함할지 결정가능)
        GameObject[] obj7 = GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        // FindObjectOfType : 비권장

        // 컴포넌트 찾기
        Transform t = GetComponent<Transform>();    // = this.transform;
        
        // 컴포넌트 추가하기
        this.gameObject.AddComponent<Test>();

        // Resources 폴더 사용하기
        Texture tex = Resources.Load<Texture>("grass-texture-26");
        Sprite sprite = Resources.Load<Sprite>("AAA/grass-Sprite-26"); 
    }

    // Update is called once per frame
    void Update()
    {
        // InputManager : Polling 방식 -> Busy wait -> 성능저하 + CPU가 sleep 상태로 들어갈 수 없게됨
        if(Input.GetKeyDown(KeyCode.W))
        {
            // InputManager : 매 프레임마다 키입력 상태를 확인하고 필요한 처리를 수행
        }

        if(Input.GetButton("Jump")) // 스페이스키가 눌려져 있는가?
        {
            // Project Setting -> Input Manager 항목에 버튼 이름들이 어떤 키와 연결되어있는지 설정되어 있음
        }

        if(Input.GetAxis("Horizontal") > 0)
        {
            // 오른쪽 방향(D)가 눌려져 있다.
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            // 왼쪽 방향(A)가 눌려져 있다.
        }
        else
        {
            // 중립 상태
        }

        // InputSystem : Event-Driven 방식으로 구현됨

    }
}
