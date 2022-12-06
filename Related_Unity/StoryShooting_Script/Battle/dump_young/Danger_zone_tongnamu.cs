using UnityEngine;
using System.Collections;

public class Danger_zone_tongnamu : MonoBehaviour {
    public GameObject tong_namu;
    public float time;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        if(time<=0.0f)
        {
            Instantiate(tong_namu, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}
}
