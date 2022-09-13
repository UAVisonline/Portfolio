using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SelectCamera : MonoBehaviour
{
    [BoxGroup("Reference")] [SerializeField] private Skybox skybox;

    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private float material_rotation;
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private float material_exposure;

    [BoxGroup("Variable")] [SerializeField] private float rotate_constant;
    [BoxGroup("Variable")] [SerializeField] private float exposure_constant;
    [BoxGroup("Variable")] [SerializeField] private float min_exposure;
    [BoxGroup("Variable")] [SerializeField] private float max_exposure;
    private bool exposure_to_max = true;

    private void Awake()
    {
        skybox = this.GetComponent<Skybox>();

        if (skybox.material.HasProperty("_Rotation"))
        {
            skybox.material.SetFloat("_Rotation", 0.0f);
        }

        if (skybox.material.HasProperty("_Exposure"))
        {
            skybox.material.SetFloat("_Exposure", min_exposure);
        }
        //material_exposure = skybox.material.GetFloat("_Exposure");
        //material_rotation = skybox.material.GetFloat("_Rotation");
    }

    private void Update()
    {
        Update_rotation();
        Update_exposure();
    }

    private void Update_rotation()
    {
        material_rotation += Time.deltaTime * rotate_constant;
        if (material_rotation >= 360.0f)
        {
            material_rotation -= 360.0f;
        }

        if (skybox.material.HasProperty("_Rotation"))
        {
            skybox.material.SetFloat("_Rotation", material_rotation);
        }
    }

    private void Update_exposure()
    {
        if(exposure_to_max)
        {
            material_exposure += Time.deltaTime * exposure_constant;
            if(material_exposure> max_exposure)
            {
                material_exposure = max_exposure;
                exposure_to_max = false;
            }
        }
        else
        {
            material_exposure -= Time.deltaTime * exposure_constant;
            if (material_exposure < min_exposure)
            {
                material_exposure = min_exposure;
                exposure_to_max = true;
            }
        }

        if (skybox.material.HasProperty("_Exposure"))
        {
            skybox.material.SetFloat("_Exposure", material_exposure);
        }
    }
}
