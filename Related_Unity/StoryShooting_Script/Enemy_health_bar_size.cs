using UnityEngine;
using System.Collections;

public class Enemy_health_bar_size : MonoBehaviour {

    public float width_divide;
    //public Camera cam;
	// Use this for initialization
	void Start () {
        
        RectTransform health_bar = GetComponent<RectTransform>();
        health_bar.sizeDelta = new Vector2(Screen.width/width_divide, 45); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
