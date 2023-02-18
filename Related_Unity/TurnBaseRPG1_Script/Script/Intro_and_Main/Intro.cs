using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Intro : MonoBehaviour, IPointerClickHandler
{
    private AudioSource audiosource;

    public List<string> intro_string;
    public float original_text_regen_time;
    public float origianl_text_change_time;

    [SerializeField] private TextMeshProUGUI intro_text;
    private int string_row_pos = 0;
    private int string_col_pos = 0;
    private float text_regen_time, text_change_time;
    private bool text_bool = false;

    [SerializeField] private TextMeshProUGUI skip_text;
    private bool skip = false;
    private float skip_text_timing = -1.0f;
    private float skip_timing = -1.0f;

    [SerializeField] private Image skip_panel;
    private float panel_fill = 0.0f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(skip==false)
        {
            if (skip_timing < 0.0f)
            {
                skip_text.color = new Color(skip_text.color.r, skip_text.color.g, skip_text.color.b, 1.0f);
                skip_timing = 0.3f;
                skip_text_timing = 1.5f;
            }
            else
            {
                skip = true;
            }
        }
    }

    void Awake()
    {
        audiosource = this.GetComponent<AudioSource>();
    }

    private void Start()
    {
        text_regen_time = original_text_regen_time;
        text_change_time = origianl_text_change_time;
    }

    private void Update()
    {
        if(skip==false)
        {
            if(skip_text_timing > 0.0f)
            {
                skip_text_timing -= Time.deltaTime;
                if(skip_text_timing<=0.0f)
                {
                    skip_text.color = new Color(skip_text.color.r, skip_text.color.g, skip_text.color.b, 0.0f);
                }
            }

            if(skip_timing >0.0f)
            {
                skip_timing -= Time.deltaTime;
            }

            if (text_change_time > 0.0f)
            {
                text_change_time -= Time.deltaTime;
                return;
            }

            if (text_regen_time > 0.0f)
            {
                text_regen_time -= Time.deltaTime;
            }
            else
            {
                if (text_bool == false)
                {
                    if (string_row_pos < intro_string.Count)
                    {
                        if (string_col_pos < intro_string[string_row_pos].Length)
                        {
                            if(intro_string[string_row_pos][string_col_pos]!=' ')
                            {
                                audiosource.Play();
                            }
                            intro_text.text += intro_string[string_row_pos][string_col_pos];
                            string_col_pos++;
                        }
                        else
                        {
                            text_change_time = origianl_text_change_time;
                            string_row_pos++;
                            text_bool = true;
                        }
                    }
                    else
                    {
                        SceneManagerCode.sceneManagerCode.Scene_move("Main");
                    }
                }
                else
                {
                    if (intro_text.text.Length > 0)
                    {
                        if(intro_text.text[intro_text.text.Length-1]!=' ')
                        {
                            audiosource.Play();
                        }
                        intro_text.text = intro_text.text.Substring(0, intro_text.text.Length - 1);
                        audiosource.Play();
                    }
                    else
                    {
                        text_bool = false;
                        string_col_pos = 0;
                        if (string_row_pos >= intro_string.Count)
                        {
                            text_change_time = origianl_text_change_time;
                        }
                    }
                }
                text_regen_time = original_text_regen_time;
            }
        }
        else
        {
            if(panel_fill<3.0f)
            {
                panel_fill += Time.deltaTime * 2.0f;
                skip_panel.fillAmount = panel_fill;
            }
            else
            {
                SceneManagerCode.sceneManagerCode.Scene_move("Main");
            }
        }
    }
}
