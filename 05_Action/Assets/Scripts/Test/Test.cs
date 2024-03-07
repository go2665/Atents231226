using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
