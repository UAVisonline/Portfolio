using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpectrumVisual2 : MonoBehaviour
{
    private float cur_value;

    [BoxGroup("Spectrum")] [SerializeField] private float spectrum_value_line;
    [BoxGroup("Spectrum")] [SerializeField] private float recheck_value_time;
    [BoxGroup("Spectrum")] [SerializeField] private float changing_value_time;

    [BoxGroup("Visual")] [SerializeField] private Vector3 original_visual_size;
    [BoxGroup("Visual")] [SerializeField] private Vector3 change_visual_size;
    [BoxGroup("Visual")] [SerializeField] private float lerp_up_value;
    [BoxGroup("Visual")] [SerializeField] private float lerp_down_value;

    [BoxGroup("Reference")] [SerializeField] private Animator animator;
    [BoxGroup("Reference")] [SerializeField] private DirectGameObject direct;

    private float original_spectrum_value_line;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        original_spectrum_value_line = spectrum_value_line;
        transform.localScale = original_visual_size;
        timer = recheck_value_time;
        animator = this.GetComponent<Animator>();
        direct = this.GetComponent<DirectGameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        cur_value = SpectrumData.spectrum_value;
        

        if (recheck_value_time <= timer)
        {
            if (cur_value > spectrum_value_line)
            {
                if (animator != null && direct == null)
                {
                    animator.Play("Play", -1, 0.0f);
                }
                timer = 0.0f;
            }
        }

        if (timer < changing_value_time)
        {
            transform.localScale = Vector3.Lerp(this.transform.localScale, change_visual_size, lerp_up_value * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(this.transform.localScale, original_visual_size, lerp_down_value * Time.deltaTime);
        }

        timer += Time.deltaTime;
    }

    public void change_visual_original()
    {
        spectrum_value_line = original_spectrum_value_line;
    }

    public void set_visual(float value)
    {
        spectrum_value_line = value;
    }
}
