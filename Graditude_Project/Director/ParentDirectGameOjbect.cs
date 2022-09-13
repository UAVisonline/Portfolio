using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParentDirectGameOjbect : DirectGameObject
{
    [BoxGroup("Reference")] [SerializeField] private List<DirectGameObject> ChildDirectObject;

    public override void play_animation(string name)
    {
        //Debug.Log(this.gameObject.name);
        //Debug.Log("Parent Direct : " + name);
        for(int i =0;i<ChildDirectObject.Count;i++)
        {
            ChildDirectObject[i].play_animation(name);
        }
    }

    public override void function0()
    {
        for (int i = 0; i < ChildDirectObject.Count; i++)
        {
            ChildDirectObject[i].function0();
        }
    }

    public override void function1()
    {
        for (int i = 0; i < ChildDirectObject.Count; i++)
        {
            ChildDirectObject[i].function1();
        }
    }

    public override void function2()
    {
        for (int i = 0; i < ChildDirectObject.Count; i++)
        {
            ChildDirectObject[i].function2();
        }
    }

    public override void function3()
    {
        for (int i = 0; i < ChildDirectObject.Count; i++)
        {
            ChildDirectObject[i].function3();
        }
    }

    public override void function4()
    {
        for (int i = 0; i < ChildDirectObject.Count; i++)
        {
            ChildDirectObject[i].function4();
        }
    }
}
