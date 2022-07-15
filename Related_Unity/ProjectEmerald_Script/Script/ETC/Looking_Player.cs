using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking_Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player_Controller.player_controller.transform.position.x > this.transform.position.x)
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f,1.0f);
            transform.GetChild(0).transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            this.transform.localScale = new Vector3(-1.0f, 1.0f,1.0f);
            transform.GetChild(0).transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }
}
