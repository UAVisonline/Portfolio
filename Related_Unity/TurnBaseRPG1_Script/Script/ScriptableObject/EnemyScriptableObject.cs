using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Drop_information
{
    [SerializeField] public List<Item_information> item_information;

    [SerializeField] public float item_drop_percent;
}

[CreateAssetMenu(menuName = "ScriptableObject/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public CharacterSpec enemy_spec;

    [SerializeField] private int code;
    [SerializeField] private int alive_number = 0;
    [SerializeField] private List<Drop_information> drop_item;
    
    public int ret_code()
    {
        return code;
    }

    public int ret_alive_number()
    {
        return alive_number;
    }

    public List<Item_information> ret_drop_item_information()
    {
        List<Item_information> value = new List<Item_information>();

        for(int i=0;i<drop_item.Count;i++)
        {
            float rand = Random.Range(0.0f, 1.0f);
            if(rand <= drop_item[i].item_drop_percent)
            {
                int count = drop_item[i].item_information.Count;
                float item_rand = Random.Range(0.0f, 1.0f);

                int pos = Mathf.FloorToInt(item_rand * count);
                Debug.Log(pos + " : " + count);

                if(pos>=count)
                {
                    pos = count - 1;
                }
                value.Add(drop_item[i].item_information[pos]);
            }
        }

        return value;
    }
}
