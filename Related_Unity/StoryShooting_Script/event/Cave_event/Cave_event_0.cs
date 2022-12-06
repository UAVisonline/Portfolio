using UnityEngine;
using System.Collections;

public class Cave_event_0 : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(PlayerPrefs.GetInt("game_save")==1)//게임의 세이브데이터가 있다면
        {
            gameObject.SetActive(false);//해당 게임오브젝트를 가동종료함.
        }
	}
}
