using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletManager : MonoBehaviour
{
    private static AmuletManager _amuletmanager;

    public static AmuletManager amuletmanager
    { 
        get
        {
            if(_amuletmanager==null)
            {
                _amuletmanager = FindObjectOfType<AmuletManager>();
                if(_amuletmanager==null)
                {
                    Debug.LogError("Can't Find Amulet Manager");
                }
            }
            return _amuletmanager;
        }
    }

    public List<GameObject> amulet_resources;
    public Dictionary<int, GameObject> amulet_dictionary = new Dictionary<int, GameObject>();
    private List<int> amulet_code_list = new List<int>();

    private void Awake()
    {
        if(_amuletmanager==null)
        {
            DontDestroyOnLoad(this.gameObject);
            _amuletmanager = this;

            for (int i = 0; i < amulet_resources.Count; i++)
            {
                BaseAmuletScript temp = amulet_resources[i].GetComponent<BaseAmuletScript>();
                amulet_dictionary.Add(temp.information.code, amulet_resources[i]);
                amulet_code_list.Add(temp.information.code);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void insert_carried_amulet(int code)
    {
        Instantiate(amulet_dictionary[code], this.transform.position, Quaternion.identity, this.transform);
    }

    public void insert_uncarried_amulet(int code)
    {
        if(amulet_dictionary.ContainsKey(code))
        {
            GameObject tmp = Instantiate(amulet_dictionary[code], this.transform.position, Quaternion.identity, this.transform);

            BaseAmuletScript script = tmp.GetComponent<BaseAmuletScript>();
            if (script != null)
            {
                script.OnAcquire();
            }
        }
    }

    public bool is_amulet_here(int code)
    {
        if(amulet_dictionary.ContainsKey(code)==true)
        {
            return true;
        }
        return false;
    }

    public Amulet_information get_amulet_information(int code)
    {
        return amulet_dictionary[code].GetComponent<BaseAmuletScript>().information;
    }

    public int ret_amulet_code_list_size()
    {
        return amulet_code_list.Count;
    }

    public int ret_amulet_code_list_pos(int pos)
    {
        return amulet_code_list[pos];
    }
}
