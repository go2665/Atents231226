using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TestData : IComparable<TestData>
{
    int x;
    float y;
    string z;

    public TestData(int x, float y, string z)
    {
        // 생성자 만들기(x,y,z값 받기)
        this.x = x; 
        this.y = y; 
        this.z = z;
    }

    public int CompareTo(TestData other)
    {
        // TestData의 리스트에서 Sort함수를 사용할 수 있게 만들기(기준은 z, 내림차순)
        //if(other == null) return 1;

        return other.z.CompareTo(this.z);
        //return z.CompareTo(other.z);
        //return x.CompareTo(other.x);
    }

    public static bool operator == (TestData left, TestData right)
    {
        // == 명령어 오버로딩하기(x값이 같으면 같다)
        return left.x == right.x;
    }

    public static bool operator != (TestData left, TestData right)
    {
        return left.x != right.x;
    }

    public override bool Equals(object obj)
    {
        // obj는 Node 클래스고 this와 obj의 x, y가 같다.
        return obj is TestData other && this.x == other.x;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z); 
    }

}
