using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    // 로딩 스크린
    // 1. 씬 로딩 진행도에 따라 슬라이더의 value가 변경된다(100%일 때 1)
    // 2. 로딩 중에는 LoadingText의 글자가 "Loading .", "Loading . .", "Loading . . .", "Loading . . . .", "Loading . . . . ."가 계속 반복된다.
    // 3. 로딩이 완료되면 LoadingText의 글자가 "Loading Complete!"로 변경되고 PressText가 활성화 된다.
    // 4. 로딩 진행도는 onMazeGenerated가 실행되었을 때 70%, onSpawnComplete가 실행되었을 때 100%
    // 5. 로딩이 완료되었을 때 아무 키보드 입력이나 마우스 클릭이 입력되면 로딩 스크린이 비활성화 된다.
    // 6. 씬 로딩 진행도는 목표치까지 꾸준히 증가한다.

}
