using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmallinformationWindow : MonoBehaviour
{
    [SerializeField] private RectTransform rect;

    [SerializeField] private RectTransform title_rect;
    [SerializeField] private RectTransform information_rect;

    [SerializeField] private TextMeshProUGUI title_text;
    [SerializeField] private TextMeshProUGUI information_text;

    private void OnEnable()
    {
        if(rect==null)
        {
            rect = this.GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rect!=null)
        {
            rect.position = Input.mousePosition;
        }
    }

    public void visualize(string title, string information)
    {
        title_text.text = title;
        information_text.text = information;

        title_text.color = new Color(title_text.color.r, title_text.color.g, title_text.color.b, 0.0f);
        information_text.color = new Color(information_text.color.r, information_text.color.g, information_text.color.b, 0.0f);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0.0f);

        StartCoroutine("window_set");
    }

    IEnumerator window_set()
    {
        yield return null;

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, title_rect.sizeDelta.y + information_rect.sizeDelta.y + 15.0f);
        title_text.color = new Color(title_text.color.r, title_text.color.g, title_text.color.b, 1.0f);
        information_text.color = new Color(information_text.color.r, information_text.color.g, information_text.color.b, 1.0f);
    }
}
