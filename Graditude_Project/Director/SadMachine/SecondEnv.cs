using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEnv : DirectGameObject
{
    [SerializeField] private Material material;

    [SerializeField] private Color original_color;
    [SerializeField] private Color first_color;
    [SerializeField] private Color second_color;
    [SerializeField] private Color third_color;
    [SerializeField] private Color fourth_color;

    void OnEnable()
    {
        material.SetColor("_ShapeColor", original_color);
    }

    public override void function1()
    {
        material.SetColor("_ShapeColor", first_color);
    }

    public override void function2()
    {
        material.SetColor("_ShapeColor", second_color);
    }

    public override void function3()
    {
        material.SetColor("_ShapeColor", third_color);
    }

    public override void function4()
    {
        material.SetColor("_ShapeColor", fourth_color);
    }
}
