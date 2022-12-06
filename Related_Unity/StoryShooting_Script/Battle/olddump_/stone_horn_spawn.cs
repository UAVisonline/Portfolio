using UnityEngine;
using System.Collections;

public class stone_horn_spawn : MonoBehaviour {
    public GameObject stone_horn;
    public bool only_one_spawn;
    public float spawn_Time;
    private float original_spawn_Time;
	// Use this for initialization
	void Start () {
        original_spawn_Time = spawn_Time;
	}
	
	// Update is called once per frame
	void Update () {
        spawn_Time -= Time.deltaTime;
        if(spawn_Time<0.0f)
        {
            spawn_Time = original_spawn_Time;
            Instantiate(stone_horn, transform.position, Quaternion.identity);
            if(only_one_spawn)
            {
                Destroy(gameObject);
            }
        }
	}
}
