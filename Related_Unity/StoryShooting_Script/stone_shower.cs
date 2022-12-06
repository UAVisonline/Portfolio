using UnityEngine;
using System.Collections;

public class stone_shower : MonoBehaviour {
    public float down_time, down_distance;
    private float translate_distance;
	// Use this for initialization
	void Start () {
	    if(down_time!=0.0f && down_distance!=0.0f)
        {
            translate_distance = down_time / down_distance ;
            Debug.Log(translate_distance * down_distance);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(down_time>0.0f)
        {
            down_time -= Time.deltaTime;
            transform.Translate(new Vector2(0.0f,down_distance*Time.deltaTime));
        }
        
	}
}
