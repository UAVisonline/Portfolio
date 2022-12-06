using UnityEngine;
using System.Collections;

public class DonotDestory : MonoBehaviour {

    public static bool object_exist = false;
	// Use this for initialization
	void Start () {
	    if(!object_exist)
        {
            object_exist = true;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
