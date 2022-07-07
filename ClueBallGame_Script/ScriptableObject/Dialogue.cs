using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObject/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private List<string> context; // 대화 Context

    public string get_string() // 대화(String 배열)를 String 변수로 반환
    {
        string answer = "";
        for(int i =0;i<context.Count;i++)
        {
            answer += context[i];
            if(i!=context.Count-1)
            {
                answer += '\n';
            }
        }

        return answer;
    }
}
