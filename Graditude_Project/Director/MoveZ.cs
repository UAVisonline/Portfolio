using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZ : MonoBehaviour
{
    [SerializeField] private Transform back;
    [SerializeField] private Transform front;

    [SerializeField] private float move_speed;


    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0.0f, 0.0f, move_speed * Time.deltaTime);
        if(this.transform.position.z < back.position.z)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, front.position.z);
        }
    }
}
