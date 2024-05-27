using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllerCell : Cell
{
    public int setGroup;
    const int NotSet = -1;
    public EllerCell(int x, int y) : base(x, y)
    {
        setGroup = NotSet;
    }
}

public class Eller : Maze
{
    protected override void OnSpecificAlgorithmExcute()
    {
        /// 1. 한 줄 만들기
        ///     1.1. 위쪽 줄을 참조해서 한 줄 만들기
        ///         1.1.1. 위쪽 셀에 벽이 없으면 위쪽 셀과 같은 집합에 포함시킨다.
        ///         1.1.2. 위쪽 셀에 벽이 있으면 새 집합에 포함시킨다.
        ///     1.2. 첫번째 줄은 가로 길이만큼 셀을 만들고 각각 고유한 집합에 포함시킨다.
        /// 2. 옆칸끼리 합치기
        ///     2.1. 서로 집합이 다르면 랜덤하게 벽을 제거하고 같은 집합으로 만든다.(같은 줄에 있는 같은 종류의 셀이 한번에 바뀐다.)
        ///     2.2. 서로 같은 집합이면 패스
        /// 3. 아래쪽 벽 제거하기
        ///     3.1. 같은 집합당 최소 하나 이상의 벽이 제거되어야 한다.
        /// 4. 한 줄 완료 -> 1번으로 돌아가기
        /// 5. 마지막 줄 만들고 정리
        ///     5.1. 생성까진 똑같이 처리
        ///     5.2. 합칠 때 세트가 다르면 무조건 합친다.
    }
}
