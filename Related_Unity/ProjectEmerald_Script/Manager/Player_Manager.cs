using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    private int player_hp, player_stamina,limit_hp,limit_stamina;
    private int Item_Frecuency, limit_Item_Frecuency;
    private float stamina_regen_time;
    [SerializeField] private float position_x, position_y;
    [SerializeField] private bool player_left;
    private static Player_Manager _player_manager;
    private static bool Destory_value;

    public static Player_Manager player_manager
    {
        get
        {
            if (_player_manager == null)
            {
                _player_manager = FindObjectOfType<Player_Manager>();
                if (_player_manager == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _player_manager;
        }
    }

    private void Awake()
    {
        if(_player_manager==null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else if(_player_manager!=null)
        {
            Destroy(this.gameObject);
        }

        limit_hp = 5;
        limit_stamina = 20;
        limit_Item_Frecuency = 3;
        player_hp = limit_hp;
        player_stamina = limit_stamina;
        Item_Frecuency = limit_Item_Frecuency;
        

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player_stamina<limit_stamina)
        {
            stamina_regen_time += Time.deltaTime;
            if(stamina_regen_time>=0.5f)
            {
                stamina_regen_time = 0.0f;
                stamina_caculate(1);
            }
        }
        //Debug.Log(player_stamina);
    }

    public int hp_return()
    {
        return player_hp;
    }

    public int return_limit_hp()
    {
        return limit_hp;
    }

    public int stamin_return()
    {
        return player_stamina;
    }

    public int return_limit_stamina()
    {
        return limit_stamina;
    }

    public int frecuency_return()
    {
        return Item_Frecuency;
    }

    public void hp_caculate(int number)
    {
        player_hp += number;
        if(player_hp>limit_hp)
        {
            player_hp = limit_hp;
        }
        HP_UI[] ui = FindObjectsOfType<HP_UI>();
        if(ui!=null)
        {
            for (int i = 0; i < ui.Length; i++)
            {
                ui[i].hp_set();
            }
        }
    }

    public void stamina_caculate(int number)
    {
        player_stamina += number;
        if (player_stamina > limit_stamina)
        {
            player_stamina = limit_stamina;
        }
        Stamina_UI[] ui = FindObjectsOfType<Stamina_UI>();
        if(ui!=null)
        {
            for (int i = 0; i < ui.Length; i++)
            {
                ui[i].stamina_set();
            }
        }
    }

    public void Item_frecuency_caculate(int number)
    {
        Item_Frecuency += number;
        if(Item_Frecuency>limit_Item_Frecuency)
        {
            Item_Frecuency = limit_Item_Frecuency;
        }
        string str = Item_Frecuency.ToString();
        Item_UI item = FindObjectOfType<Item_UI>();
        if(item!=null)
        {
            item.Item_set(str);
        }   
    }

    public void Refill()
    {
        hp_caculate(limit_hp - player_hp);
        stamina_caculate(limit_stamina - player_stamina);
        Item_frecuency_caculate(limit_Item_Frecuency - Item_Frecuency);
        //player_hp = limit_hp;
        //player_stamina = limit_stamina;
        //Item_Frecuency = limit_Item_Frecuency;
    }

   public float x_return()
   {
        return position_x;
   }
   
    public float y_return()
    {
        return position_y;
    }

    public bool left_return()
    {
        return player_left;
    }

    public void set_position(float x, float y, bool left)
    {
        position_x = x;
        position_y = y;
        player_left = left;
    }
}
