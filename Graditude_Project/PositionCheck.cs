using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCheck : MonoBehaviour // Objective of this code : Just Debug
{

    [SerializeField] private Transform left;
    [SerializeField] private Transform right;

    private float x_min, x_max;
    private float y_min, y_max;


    private void Start()
    {
        x_min = 0.0f;
        y_min = 0.0f;
        x_max = 0.0f;
        y_max = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(x_min >= left.position.x)
        {
            x_min = left.position.x;
        }
        
        if(x_max <= right.position.x)
        {
            x_max = right.position.x;
        }

        if(y_min >= left.position.y)
        {
            y_min = left.position.y;
        }

        if(y_max <= right.position.y)
        {
            y_max = right.position.y;
        }

        Debug.Log("x_min : " + x_min + " /// x_max : " + x_max);
        Debug.Log("y_min : " + y_min + " /// y_max : " + y_max);
    }
}
