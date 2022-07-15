using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRealRoom : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject position_ref;

    private void OnEnable()
    {
        this.GetComponent<Animator>().SetBool("Start", true);
        this.GetComponent<Animator>().SetBool("Start", false);

        GameObject tmp = ShopManager.shopmanager.get_director(); // 현재 장착중인 Skillmode로부터 Player 연출용 Object를 받아옴 
        Instantiate(tmp, position_ref.transform.position, Quaternion.identity, parent.GetComponent<RectTransform>()); // 해당 Object를 position ref 부분에 생성

        //Debug.Log(tmp.GetComponent<RectTransform>().position);
        //tmp.GetComponent<RectTransform>().transform.position += new Vector3(0.0f, 150.0f,0.0f);
        //Debug.Log(tmp.GetComponent<RectTransform>().position);
    }
}
