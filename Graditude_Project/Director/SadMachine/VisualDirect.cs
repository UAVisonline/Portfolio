using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDirect : DirectGameObject
{
    [SerializeField] private SpectrumVisual visual;
    [SerializeField] private float first_visual_line;
    [SerializeField] private float second_visual_line;
    [SerializeField] private float third_visual_line;
    [SerializeField] private float fourth_visual_line;

    public override void function0()
    {
        visual.change_visual_original();
    }

    public override void function1()
    {
        visual.set_visual(first_visual_line);
    }

    public override void function2()
    {
        visual.set_visual(second_visual_line);
    }

    public override void function3()
    {
        visual.set_visual(third_visual_line);
    }

    public override void function4()
    {
        visual.set_visual(fourth_visual_line);
    }
}
