using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CubeScript : DirectGameObject
{
    private bool size_small = false;
    private float wait_time = 0.0f;
    [SerializeField] private float small_constant;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] Vector3 size;

    private void Start()
    {
        //size = this.transform.localScale;
    }

    private void Update()
    {
        if (size_small == true)
        {
            if (wait_time <= 0.0f)
            {
                float constant = small_constant * Time.deltaTime;
                if (constant > this.transform.localScale.x)
                {
                    this.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                }
                else
                {
                    this.transform.localScale = this.transform.localScale - new Vector3(constant, constant, constant);
                }
                wait_time = Time.deltaTime;
            }
            else
            {
                wait_time -= Time.deltaTime;
            }
        }

        if (this.transform.localScale.x <= 0.0f)
        {
            this.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            particle.Stop();
            size_small = false;
        }
    }

    public override void function0() // smaller cube
    {
        //this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        this.transform.localScale = size;
        size_small = true;
    }

    public override void function1() // original cube size
    {
        //this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        this.transform.localScale = size;
        size_small = false;
        particle.Play();
    }
}
