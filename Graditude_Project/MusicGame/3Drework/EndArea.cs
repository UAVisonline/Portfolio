using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ReworkNote>()!=null)
        {
            other.GetComponent<ReworkNote>().note_interaction_uncorrect();
        }
    }
}
