using UnityEngine;
using System.Collections;

public class Time_destroy : MonoBehaviour {
    public float destroy_time;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        destroy_time -= Time.deltaTime;
        if(destroy_time<= 0.0f)
        {
            Destroy(gameObject);
        }
	}
}
