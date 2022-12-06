using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
    public GameObject shield_break;
    public int health;
    public float time;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        if(time <=0.0f)
        {
            if(shield_break!=null)
            {
                Instantiate(shield_break, transform.position, Quaternion.identity);
            } 
            Destroy(gameObject);
        }
	    if(health<=0)
        {
            if(shield_break!=null)
            {
                Instantiate(shield_break, transform.position, Quaternion.identity);
            } 
            Destroy(gameObject);
        }
	}


}
