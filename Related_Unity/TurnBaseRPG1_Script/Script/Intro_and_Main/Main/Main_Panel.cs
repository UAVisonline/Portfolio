using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main_Panel : MonoBehaviour
{
    public float original_text_time;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;

    private bool text_bool = false;
    private float text_time;

    private void Start()
    {
        text_time = 0.0f;
        image = this.GetComponent<Image>();
    }

    private void Update()
    {
        if(text_bool==false)
        {
            if(text_time<original_text_time + 0.5f)
            {
                text_time += Time.deltaTime;
                text.color = new Color(text.color.r, text.color.g, text.color.b, text_time / original_text_time);
            }
            else
            {
                text_bool = true;
            }
        }
        else
        {
            if(text_time > -0.5f)
            {
                text_time -= Time.deltaTime;
                text.color = new Color(text.color.r, text.color.g, text.color.b, text_time / original_text_time);
            }
            else
            {
                if(image.fillAmount > 0.0f)
                {
                    image.fillAmount -= Time.deltaTime * 2.0f;
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
