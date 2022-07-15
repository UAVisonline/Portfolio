using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passing_Ground : MonoBehaviour
{
    private BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player_Controller.player_controller.transform.position.y>this.transform.position.y)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }
}
