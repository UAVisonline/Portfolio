using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;
    private bool rotate = false;

    private void Start()
    {
        if(x!=0 || y!=0 || z!=0)
        {
            rotate = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate == true)
        {
            this.transform.Rotate(new Vector3(x, y, z) * Time.deltaTime);
        }
    }
}
