using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dramatic : MonoBehaviour // 화면 전환을 위해 만든 Script, 그러나 과제 시간에 쫓겨 미사용
{
    private static Dramatic _dramatic;

    public static Dramatic dramatic
    {
        get
        {
            if (_dramatic == null)
            {
                _dramatic = FindObjectOfType<Dramatic>();
            }
            return _dramatic;
        }
    }

    [SerializeField] private Animator animator;

    [SerializeField] private RandomText randomText;

    private WaitForSeconds wait_time = new WaitForSeconds(1.0f);

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void set_true()
    {
        randomText.init_text();
        animator.SetBool("Anim", true);
    }

    public void set_false()
    {
        animator.SetBool("Anim", false);
    }

    public void move_end_field()
    {
        StartCoroutine("Move_end_field");
    }

    IEnumerator Move_end_field()
    {

        yield return wait_time;
        Dramatic.dramatic.set_false();
    }
}
