using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocaObject : ScriptableObject // 스크립터블 오브젝트 - 영단어
{
    public string head; // 영어단어
    public string body; // 실제 뜻
    public float level; // 해당 단어의 level
    public bool check_it; // 이 단어를 Test에서 사용했는가?

    public string get_head()
    {
        return head;
    }

    public string get_body()
    {
        return body;
    }

    public float get_level()
    {
        return level;
    }

    public bool get_check()
    {
        return check_it;
    }
}
