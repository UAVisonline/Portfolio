using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]

public class Dialogue : ScriptableObject
{
    [TextArea(10, 10)] [SerializeField] string storyText;

    public string[] GetDialogue()
    {
        return storyText.Split('\n');
    }
}
