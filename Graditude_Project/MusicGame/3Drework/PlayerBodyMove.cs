using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerBodyMove : MonoBehaviour
{
    [BoxGroup("position")] [ReadOnly] [SerializeField] private Vector3 before_position;
    [BoxGroup("position")] [ReadOnly] [SerializeField] private Vector3 current_position;

    [BoxGroup("Value")] [SerializeField] private float minimum_velocity;

    [BoxGroup("Reference")] [SerializeField] private PlaneArea planeArea;

    private float test_value;

    private void Start()
    {
        before_position = this.transform.position;
        if(planeArea==null)
        {
            planeArea = FindObjectOfType<PlaneArea>();
        }
    }

    private void FixedUpdate()
    {
        current_position = this.transform.position;

        float x_distance = Mathf.Abs(current_position.x - before_position.x);

        float velocity = x_distance / Time.deltaTime;

        if(minimum_velocity < velocity)
        {
            Debug.Log(velocity);
            if (current_position.x > before_position.x)
            {
                planeArea.moving_player(1);
                //Debug.Log("Left");
            }
            else if (current_position.x < before_position.x)
            {
                planeArea.moving_player(-1);
                //Debug.Log("Right");
            }
        }

        before_position = current_position;
    }
}
