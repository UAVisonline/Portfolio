using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directer_machine : MonoBehaviour
{
    private static Directer_machine _directer;
    private Animator animator;
    bool next_panel;

    public static Directer_machine directer // Singleton 설정
    {
        get
        {
            if (_directer == null)
            {
                _directer = FindObjectOfType<Directer_machine>();
                if (_directer == null)
                {
                    Debug.LogError("Can't Load Problem");
                }
            }
            return _directer;
        }
    }

    private void Awake() // Object 첫 생성 시 Singleton 할당, 그 외에 경우 Object 삭제
    {
        if(_directer==null)
        {
            DontDestroyOnLoad(this.gameObject);
            _directer = FindObjectOfType<Directer_machine>();
            if (_directer == null)
            {
                Debug.LogError("Can't Load Problem");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        next_panel = false;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Problem_next_on", next_panel);
    }

    public void set_panel_slide(bool status) // next panel boolean 설정 (true->화면 어두워짐, false->다시 밝아짐)
    {
        next_panel = status;
    }
}
