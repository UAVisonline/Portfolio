using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString(this.gameObject.name)=="true")
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void open()
    {
        this.GetComponent<Animator>().Play("Open");
    }

    private void going_up()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 2.0f);
    }

    private void move_stop()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
