using UnityEngine;
using System.Collections;

public class meteor_making : MonoBehaviour {

    public GameObject meteor_1_go_left,meteor_2_go_right;
    public float child_time;
    private float original_child_time;
    public int shoot_num;
    private int num;
    // Use this for initialization
    void Start () {
        num = 0;
        original_child_time = child_time;
	}
	
	// Update is called once per frame
	void Update () {

        if (child_time > 0.0f)
        {
            child_time -= Time.deltaTime;
        }
        else
        {
            child_time = original_child_time;
            if(num%2==0)
            {
                Instantiate(meteor_1_go_left,new Vector2(2.8f,Random.Range(1.00f,-5.00f)), transform.rotation);
            }
            else
            {
                Instantiate(meteor_2_go_right, new Vector2(-5.90f, Random.Range(1.00f, -5.00f)), transform.rotation);
            }
            num++;
            if (shoot_num==num)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
