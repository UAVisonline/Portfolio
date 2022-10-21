using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class Active_Button : MonoBehaviour
{
    [BoxGroup("Active false")] [SerializeField] private List<GameObject> false_objects;
    [BoxGroup("Active true")] [SerializeField] private List<GameObject> true_objects;

    public void function()
    {
        for(int i =0;i<false_objects.Count;i++)
        {
            false_objects[i].SetActive(false);
        }

        for(int i =0;i<true_objects.Count;i++)
        {
            true_objects[i].SetActive(true);
        }
    }
}
