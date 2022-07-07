using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomText : MonoBehaviour // Dramatic Script에서 사용
{
    [SerializeField] private TextMeshProUGUI random_text;

    public List<string> text_list;

    public void init_text()
    {
        random_text.text = text_list[Random.Range(0, text_list.Count)];
    }
}
