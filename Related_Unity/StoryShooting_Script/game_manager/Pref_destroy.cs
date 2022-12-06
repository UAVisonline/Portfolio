using UnityEngine;
using System.Collections;

public class Pref_destroy : MonoBehaviour {

    public int destory_value;
    public bool prefs_has_key_destory;//프렙이 값을 가지고 있지 않으면 파괴(false),  프렙이 일정값을 가졌을때 파과(true);
    public bool prefs_destory_correct_value;//프렙이 일정 값일때 삭제(true), 프렙이 일정 값이 아닐때 삭제(false)
    public bool prefs_high;//prefs_has_key_destory가 true일때 해당 destory_value보다 높으면 삭제//prefs_destory_correct_value가 false일떄만
    public bool prefs_low;//위 bool에 기능과 반대
    public bool not_has_delete;//프렙이 값을 안 가지면 삭제(ture),프렙이 값을 가지면 삭제(false)//prefs_has_key_destory가 false일때만
    public string prefs;
        // Use this for initialization
	void Start () {
	if(prefs_has_key_destory)
        {
            if(prefs_destory_correct_value)
            {
                if (PlayerPrefs.GetInt(prefs) == destory_value)
                {
                    Destroy(gameObject);
                }
            }
            else if(prefs_high && !prefs_destory_correct_value)
            {
                if (PlayerPrefs.GetInt(prefs) > destory_value)
                {
                    Destroy(gameObject);
                }
            }
            else if(prefs_low && !prefs_destory_correct_value)
            {
                if (PlayerPrefs.GetInt(prefs) < destory_value)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (PlayerPrefs.GetInt(prefs) != destory_value)
                {
                    Destroy(gameObject);
                }
                if(!PlayerPrefs.HasKey(prefs))
                {
                    Destroy(gameObject);
                }
            }
            
        }
        else
        {
            if(!not_has_delete)
            {
                if (PlayerPrefs.HasKey(prefs))
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if(!PlayerPrefs.HasKey(prefs))
                {
                    Destroy(gameObject);
                }
                
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
