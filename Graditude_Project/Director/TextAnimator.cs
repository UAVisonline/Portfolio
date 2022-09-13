using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class TextAnimator : DirectGameObject
{
    [BoxGroup("Reference")] [SerializeField] private Text text;
    [BoxGroup("Reference")] [SerializeField] private TextMeshProUGUI textpro;
    [BoxGroup("Reference")] [SerializeField] private Material text_material;


    // Start is called before the first frame update
    void Awake()
    {
        if(text==null)
        {
            text = this.GetComponent<Text>();
        }
        
        if(textpro==null)
        {
            textpro = this.GetComponent<TextMeshProUGUI>();
        }
    }


    public void text_set(string value)
    {
        if(text!=null)
        {
            text.text = value;
        }
        
        if(textpro!=null)
        {
            textpro.text = value;
        }
    }

    public void random_text_generate(int length)
    {
        string value = "";
        for(int i =0;i<length;i++)
        {
            char random = (char)Random.Range(65, 126);
            value += random;
        }

        if (text != null)
        {
            text.text = value;
        }

        if (textpro != null)
        {
            textpro.text = value;
        }
    }
}
