using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LightObject : MonoBehaviour // 발광 오브젝트 및 조명장치
{
    [BoxGroup("Reference")] [SerializeField] private Light light; // 조명 <이 놈은 Null값이면 안된다>
    [BoxGroup("Reference")] [SerializeField] private GameObject obj; // 발광 오브젝트
    [BoxGroup("Reference")] [ReadOnly] [SerializeField] private Material light_material; // 발광 오브젝트의 material
    [BoxGroup("Reference")] [SerializeField] private Animator animator; // Animator로 특별 효과도 줄 수 있다... 근데 아직 사용은 안 함

    private void Awake()
    {
        if(obj!=null)
        {
            light_material = obj.GetComponent<MeshRenderer>().material;
        }
        
        if(this.GetComponent<Animator>()!=null)
        {
            animator = this.GetComponent<Animator>();
        }
        
        this.gameObject.SetActive(false); // 이 놈은 기본적으로 켜져있어서, 생성하면 꺼버려야됨
    }


    // Update is called once per frame, 으악 비어있는 Update문은 성능을 잡아먹는다.
    /*
    void Update()
    {

    }
    */

    public void set_color_instant(Color color) // Light의 color 변경, 오브젝트도 있으면 오브젝트의 발광 material color도 변경
    {
        light.color = color;
        if(light_material!=null)
        {
            light_material.SetColor("_EmissionColor", color);
        }
    }

    public void play_animation(string name)
    {
        if(animator!=null)
        {
            animator.Play(name, -1, 0.0f);
        }
    }
}
