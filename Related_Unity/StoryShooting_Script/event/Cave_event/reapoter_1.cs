using UnityEngine;
using System.Collections;

public class reapoter_1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	if(PlayerPrefs.HasKey("reaper_battle"))
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
