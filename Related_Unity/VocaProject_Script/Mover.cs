using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mover : MonoBehaviour // 간단한 오브젝트 Turn On/OFF Script
{
    [SerializeField] private GameObject turn_off; // 비활성화 할 Object
    [SerializeField] private GameObject turn_on; // 활성화 할 Object

    private WaitForSeconds wait_time = new WaitForSeconds(1.0f);

    public void move_field() 
    {
        turn_on.SetActive(true);
        turn_off.SetActive(false);
        //StartCoroutine("Move_field");
    }

    IEnumerator Move_field()
    {
        Dramatic.dramatic.set_true();
        yield return wait_time;

        Dramatic.dramatic.move_end_field();
        turn_on.SetActive(true);
        turn_off.SetActive(false);
        
    }
}
