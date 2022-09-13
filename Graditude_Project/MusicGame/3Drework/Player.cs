using System;
using Oculus.Interaction;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void init_position(Vector3 pos)
    {
        transform.position += new Vector3(-pos.x, 0, transform.position.z - pos.z);
    }
}