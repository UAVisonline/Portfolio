using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voca_Mother : MonoBehaviour
{
    [SerializeField] List<Voca_index> child_index_script; // 자신 아래에 있는 Voca 내용 Script

    private void OnEnable()
    {
        reload_page();
    }

    public void reload_page()
    {
        for (int i = 0; i < child_index_script.Count; i++)
        {
            child_index_script[i].voca_init(); // 각 스크립트를 init 하는 것으로 정보 시각화
        }
    }
}
