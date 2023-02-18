using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Amulet_Scroll_View : MonoBehaviour
{
    [SerializeField] private Image amulet_jacket;
    [SerializeField] private TextMeshProUGUI amulet_name;
    [SerializeField] private TextMeshProUGUI amulet_effect;

    [SerializeField] private Transform content_transform;
    [SerializeField] private GameObject child_content;

    [SerializeField] private Amulet_information ref_information = null;

    [SerializeField] private List<Amulet_child_Content> protecteds;
    [SerializeField] private Button protected_btn;
    [SerializeField] private TextMeshProUGUI btn_text;

    private List<GameObject> child_arr = new List<GameObject>();
    private bool selected_protected = false;

    [SerializeField] private AudioClip amulet_lock;
    [SerializeField] private AudioClip amulet_unlock;

    private void OnEnable()
    {
        int amulet_count = PlayerManager.playerManager.ret_total_amulet_count();
        ref_information = null;
        selected_protected = false;

        for (int i = 0 ; i < amulet_count ; i++)
        {
            Amulet_information tmp = PlayerManager.playerManager.ret_specific_amulet_information(i);
            Amulet_child_Content child = Instantiate(child_content, content_transform).GetComponent<Amulet_child_Content>();
            child_arr.Add(child.gameObject);
            child.set_sprite(tmp.amulet_jacket);
            child.set_view(this);
            //child.set_pos(i);
            child.set_code(tmp.code);
        }

        protected_visualize();
        protected_btn.gameObject.SetActive(false);
    }

    public void Amulet_click(int code, bool protected_value)
    {
        if(code<0)
        {
            amulet_jacket.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            amulet_name.text = "";
            amulet_effect.text = "";
            protected_btn.gameObject.SetActive(false);
            return;
        }

        ref_information = AmuletManager.amuletmanager.get_amulet_information(code);
        amulet_name.text = ref_information.name;
        amulet_effect.text = ref_information.information;
        amulet_jacket.sprite = ref_information.amulet_jacket;
        amulet_jacket.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        selected_protected = protected_value;
        protected_btn.gameObject.SetActive(true);

        if (selected_protected == true) // Click - protected amulet content
        {
            btn_text.text = "보호 해제";
            protected_btn.interactable = true;
        }
        else // Click - not protected amulet content
        {
            if(PlayerManager.playerManager.full_protected_amulet()==true)
            {
                btn_text.text = "보호 불가능";
                protected_btn.interactable = false;
            }
            else
            {
                if(PlayerManager.playerManager.same_protected_amulet(code)==true)
                {
                    btn_text.text = "보호 불가능";
                    protected_btn.interactable = false;
                }
                else
                {
                    btn_text.text = "보호 설정";
                    protected_btn.interactable = true;
                }
            }
        }
    }

    public void protected_btn_function()
    {
        if (selected_protected == true) // Click - protected amulet content
        {
            PlayerManager.playerManager.delete_protected_amulet(ref_information.code);
            Util_Manager.utilManager.play_clip(amulet_unlock);
            protected_visualize();
            Amulet_click(-1, false);
        }
        else // Click - not protected amulet content
        {
            PlayerManager.playerManager.insert_protected_amulet(ref_information.code, ref_information);
            Util_Manager.utilManager.play_clip(amulet_lock);
            protected_visualize();
            Amulet_click(-1, false);
        }
    }

    private void OnDisable()
    {
        amulet_name.text = "";
        amulet_effect.text = "";
        amulet_jacket.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        for (int i =0;i<child_arr.Count;i++)
        {
            Destroy(child_arr[i]);
        }
        child_arr.Clear();
    }

    private void protected_visualize()
    {
        for (int i = 0; i < protecteds.Count; i++)
        {
            Amulet_information tmp = PlayerManager.playerManager.ret_protected_amulet_information(i);
            protecteds[i].set_view(this);

            if (tmp != null)
            {
                protecteds[i].set_sprite(tmp.amulet_jacket);
                protecteds[i].set_image_alpha(1.0f);
                protecteds[i].set_code(tmp.code);
            }
            else
            {
                protecteds[i].set_sprite(null);
                protecteds[i].set_image_alpha(0.0f);
                protecteds[i].set_code(-1);
            }
        }
    }
}
