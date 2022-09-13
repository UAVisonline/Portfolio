using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DirectGameObject : MonoBehaviour // 연출용 게임 오브젝트 <Animator를 통해 특정 연출 재생 가능 : Doom 노래에서 돌아가는 빛나는 원이라든지, 회전네모라든지는 이걸로 구현됨>
{
    [BoxGroup("Reference")] [SerializeField] private Animator animator; // 애니메이터 State 이름은 Idle, Play1~5로 설정할 것

    public virtual void play_animation(string name) 
    {
        if(animator==null)
        {
            animator = this.GetComponent<Animator>();
        }

        if (animator != null)
        {
            animator.Play(name, -1, 0.0f);
        }
    }

    public virtual void function0()
    {

    }

    public virtual void function1()
    {

    }

    public virtual void function2()
    {

    }

    public virtual void function3()
    {

    }

    public virtual void function4()
    {

    }
}
