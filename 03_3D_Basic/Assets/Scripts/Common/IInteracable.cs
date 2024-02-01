using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인터페이스 : 어떤 클래스가 반드시 이런 기능(함수)를 가지고 있다라고 명시해 놓는 것
//  - 기본적으로 모든 맴버가 public
//  - 인터페이스를 상속받은 클래스는 반드시 인터페이스의 맴버를 구현해야 한다.
//  - 인터페이스는 맴버 변수는 없다.(const는 가능)
//  - 인터페이스는 맴버 함수의 선언만 있다.(구현은 없다)
//  - 인터페이스는 상속 개수 제한이 없다.

public interface IInteracable
{
    void Use(); // 사용하는 기능이 있다고 선언해 놓은 것
}
