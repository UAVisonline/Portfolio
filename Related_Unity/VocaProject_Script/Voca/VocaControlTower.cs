using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VocaControlTower : MonoBehaviour
{
    [SerializeField] private VocaDetail detail;
    [SerializeField] private Voca_Delete delete;
    [SerializeField] private Voca_Mother mother;

    [SerializeField] private bool none;
    [SerializeField] private bool detail_bool;
    [SerializeField] private bool delete_bool;

    public void detail_btn() // 상태변환 (detail 항목을 표시)
    {
        detail.init_detail();
        detail_bool = true;
        delete_bool = false;
    }

    public void delete_btn() // 상태변환 (delete 항목을 표시)
    {
        delete.init_delete();
        delete_bool = true;
        detail_bool = false;
    }

    public void none_btn() // 상태변환 (아무런 항목도 표시하지 않음)
    {
        detail_bool = false;
        delete_bool = false;
    }

    public void init_voca()
    {
        mother.reload_page();
    }

    private void Update()
    {
        detail.animation_update(detail_bool);
        delete.animation_update(delete_bool);
    }
}
