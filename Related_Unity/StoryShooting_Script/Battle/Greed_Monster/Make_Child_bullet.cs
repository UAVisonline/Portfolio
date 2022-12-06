using UnityEngine;
using System.Collections;

public class Make_Child_bullet : MonoBehaviour {

    public GameObject child_bullet;
    public float child_time;
    private float original_child_time;
    // Use this for initialization
    void Start () {
        original_child_time = child_time;
	}
	
	// Update is called once per frame
	void Update () {
	if(child_time> 0.0f)
        {
            child_time -= Time.deltaTime;
        }
        else
        {
            child_time = original_child_time;
            Instantiate(child_bullet, transform.position, transform.rotation);
        }
	}
}
