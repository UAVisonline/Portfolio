using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Smallinformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected string name;
    [SerializeField] protected string information;

    public void OnDisable()
    {
        if(Util_Manager.utilManager.check_smallinformation(this)==true)
        {
            Util_Manager.utilManager.small_window_off();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log(eventData.position);
        Util_Manager.utilManager.small_window_init(name, information,this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Util_Manager.utilManager.small_window_off();
    }
}
