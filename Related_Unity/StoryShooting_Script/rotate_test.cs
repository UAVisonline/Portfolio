using UnityEngine;
using System.Collections;

public class rotate_test : MonoBehaviour {

    public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        
        transform.Rotate(0f, 0f, 45f);
        rb.AddForce(Vector2.down * 15);
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
