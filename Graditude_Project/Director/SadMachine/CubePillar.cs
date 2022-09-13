using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePillar : DirectGameObject
{
    [SerializeField] private int first_goal;
    [SerializeField] private int second_goal;
    private int first_var, second_var;

    [SerializeField] private Color first_color;
    [SerializeField] private Color second_color;

    private void Start()
    {
        first_var = 0;
        second_var = 0;
        this.GetComponent<MeshRenderer>().material.SetColor("_ShapeColor", Color.white);
    }

    public override void function0()
    {
        first_var += 1;
        if(first_var >= first_goal)
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_ShapeColor", first_color);
        }
    }

    public override void function1()
    {
        second_var += 1;
        if (second_var >= second_goal)
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_ShapeColor", second_color);
        }
    }
}
