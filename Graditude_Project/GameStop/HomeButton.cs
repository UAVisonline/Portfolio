using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HomeButton : MonoBehaviour
{
    [SerializeField] private GameStop gameStop;
    [SerializeField] private Animator animator;
    private bool status;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        //Debug.Log(OVRInput.Get(OVRInput.RawButton.X));

        if(OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            status = !status;
            test_home_button();
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHandObject>()!=null)
        {
            test_home_button();
        }
    }
    */

    [Button]
    public void test_home_button()
    {
        gameStop.interaction(status);
        animator.SetBool("Home", status);
    }
}
