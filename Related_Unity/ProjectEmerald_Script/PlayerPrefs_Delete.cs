using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefs_Delete : MonoBehaviour
{
    public bool if_that_have_value; // 값이 있냐, 없냐를 확인 (이에 따라 삭제 유무 결정)
    [SerializeField] private string prefs; // 값을 확인할 PlayerPref 이름

    // Start is called before the first frame update
    void Start()
    {
        if(!if_that_have_value) // PlayerPref의 값이 존재하지 않다면
        {
            if (!PlayerPrefs.HasKey(prefs)) // 실제로도 존재하지 않았다
            {
                Destroy(this.gameObject); // 오브젝트 삭제
            }
        }
        else // PlayerPref 값이 존재한다면 
        {
            if (PlayerPrefs.HasKey(prefs)) // 실제로도 존재했다
            {
                Destroy(this.gameObject);
            }
        }
    }
}
