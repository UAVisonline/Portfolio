using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DayLifescript : MonoBehaviour
{
    [SerializeField] private bool gameover_status = false;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI sentence_1_1;
    [SerializeField] private TextMeshProUGUI sentence_1_2;
    [SerializeField] private TextMeshProUGUI sentence_2;

    public void Set_sentence_mode_1()
    {
        if(PlayerManager.playerManager.spec.current_chance==1 && PlayerManager.playerManager.spec.current_turn==1)
        {
            title.text = "당신의 여정을 시작하며";
        }
        else
        {
            title.text = "당신의 여정을 돌아보며";
        }

        sentence_1_setting();
        sentence_2.text = "이 굴레를 벗어나봅시다...";
    }

    public void Set_sentence_mode_2()
    {
        title.text = "새롭게 태어난 당신의 삶...";
        sentence_1_setting();
        sentence_2.text = "죽음을 겪었지만 아직 기회는 남았습니다.";
    }

    public void Set_sentence_mode_3()
    {
        title.text = "당신의 여정이 끝나며...";
        gameover_status = true;
        sentence_1_setting();
        sentence_2.text = "만약... 그때로 돌아갈수만 있다면...";
    }

    private void sentence_1_setting()
    {
        if(gameover_status==false)
        {
            sentence_1_1.text = "지금의 당신이 지나온 나날들 : " + PlayerManager.playerManager.spec.current_turn.ToString() + " (" + PlayerManager.playerManager.spec.max_turn.ToString() + ")";
            sentence_1_2.text = "당신이 지나온 인생  : " + PlayerManager.playerManager.spec.current_chance.ToString() + " (" + PlayerManager.playerManager.spec.max_chance.ToString() + ")";
        }
        else
        {
            sentence_1_1.text = "당신은 마왕의 저주로부터 정신이 붕괴되었습니다.";
            sentence_1_2.text = "결국 미쳐버린채 영원의 삶을 보내게 되었습니다.";
        }
    }

    public void DayLifeOkayBtn()
    {
        if (gameover_status == false)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }
}
