using UnityEngine;
using System.Collections;

public class Reaper_down_bullet : MonoBehaviour {

    private float shoot_time;
    //private bool shoot;
	// Use this for initialization
	void Start () {
        shoot_time = 1.50f;
	}
	
	// Update is called once per frame
	void Update () {
	    if(shoot_time>0.0f)
        {
            shoot_time -= Time.deltaTime;
        }
        else
        {
                shoot_time += 1.00f;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.down * 60f);
            
        }
	}
}
