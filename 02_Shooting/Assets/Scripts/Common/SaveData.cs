using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// json 유틸리티에서 사용하기 위해서는 직렬화 가능한 클래스이어야 한다.
[Serializable]
public class SaveData
{
    public string[] rankerNames;
    public int[] highScores;
}
