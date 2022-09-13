using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class TextCanvas : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
