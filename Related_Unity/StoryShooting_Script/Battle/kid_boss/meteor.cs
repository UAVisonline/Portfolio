using UnityEngine;
using System.Collections;

public class meteor : MonoBehaviour {

    private Vector2 dir;
    public bool right,left;

	// Use this for initialization
	void Start () {
        if(right)
        {
            dir = new Vector2(3.00f, -1.00f);
        }
        else if(left && !right)
        {
            dir = new Vector2(-3.00f, -1.00f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 75.0f*Time.deltaTime);
	}
}
