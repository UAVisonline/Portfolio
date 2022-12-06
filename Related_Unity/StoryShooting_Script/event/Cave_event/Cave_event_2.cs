using UnityEngine;
using System.Collections;

public class Cave_event_2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if(PlayerPrefs.HasKey("greed_battle"))
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
