using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TheaterWall : DirectGameObject
{
    [SerializeField] private Material shader_material;
    [SerializeField] private Material no_change_material;

    private float shift;
    [SerializeField] private bool check_it;

    // Start is called before the first frame update
    void Start()
    {
        shift = 0.0f;

        shader_material.SetFloat("_GreyscaleBlend", 1.0f);
        shader_material.SetFloat("_HsvShift", shift);
        no_change_material.SetFloat("_GreyscaleBlend", 1.0f);
        //no_change_material.SetFloat("_HsvShift", shift);
    }

    public override void function0()
    {
        check_it = !check_it;
        if(check_it==true)
        {
            shift += 60.0f;
            if(shift>360.0f)
            {
                shift -= 360.0f;
            }
            shader_material.SetFloat("_GreyscaleBlend", 0.0f);
            no_change_material.SetFloat("_GreyscaleBlend", 0.0f);
            shader_material.SetFloat("_HsvShift", shift);
            //no_change_material.SetFloat("_HsvShift", shift);
        }
        else
        {
            shader_material.SetFloat("_GreyscaleBlend", 1.0f);
            no_change_material.SetFloat("_GreyscaleBlend", 1.0f);
        }
    }
}
