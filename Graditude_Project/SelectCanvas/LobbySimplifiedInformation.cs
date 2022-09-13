using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class LobbySimplifiedInformation : MonoBehaviour
{
    [BoxGroup("Reference")] [SerializeField] private Album_Select select_obj;

    [BoxGroup("UI reference")] [SerializeField] private TextMeshProUGUI title;
    [BoxGroup("UI reference")] [SerializeField] private Image jacket;
    
    public void SetTitleAndJacket()
    {
        title.text = select_obj.get_current_music_object().get_song_title();
        jacket.sprite = select_obj.get_current_music_object().get_album_jacket();
    }

}
