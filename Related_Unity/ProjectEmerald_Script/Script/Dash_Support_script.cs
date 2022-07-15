using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Support_script : MonoBehaviour
{
    bool ground_check;
    // Start is called before the first frame update
    void Start()
    {
        ground_check = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            ground_check = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            ground_check = false;
        }
    }

    public bool return_ground_check()
    {
        return ground_check;
    }
}
