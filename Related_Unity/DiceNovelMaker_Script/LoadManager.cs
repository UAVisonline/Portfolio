using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public delegate void no_argument_delegate();

public struct novel_element
{
    public string novel_title;
    public int image_code;

    public novel_element(string title,int code)
    {
        novel_title = title;
        image_code = code;
    }
}

public class LoadManager : SerializedMonoBehaviour
{
    private static LoadManager _loadmanager;

    public static LoadManager loadmanager
    {
        get
        {
            if(_loadmanager==null)
            {
                _loadmanager = FindObjectOfType<LoadManager>();
                if(_loadmanager==null)
                {
                    Debug.LogError("Can't Find LoadManager");
                }
            }
            return _loadmanager;
        }
    }

    public static event no_argument_delegate select_novel_event;
    public static event no_argument_delegate select_chapter_event;
    public static event no_argument_delegate select_content_event;

    [SerializeField] [ReadOnly] private List<novel_element> novel_list;

    [SerializeField] private List<Sprite> img_sprite;

    [BoxGroup("Information_Section")] [SerializeField] [ReadOnly] private string current_title;
    [BoxGroup("Information_Section")] [SerializeField] [ReadOnly] private int current_code;

    [BoxGroup("Chapter_Section")] [SerializeField] [ReadOnly] private List<string> chapter_list;
    [BoxGroup("Chapter_Section")] [SerializeField] [ReadOnly] private string current_chapter_title;
    [BoxGroup("Chapter_Section")] [SerializeField] [ReadOnly] private List<string> current_chapter_detail;

    [BoxGroup("Content_Section")] [SerializeField] [ReadOnly] private List<string> content_list;
    [BoxGroup("Content_Section")] [SerializeField] [ReadOnly] private string current_content_title;
    [BoxGroup("Content_Section")] [SerializeField] [ReadOnly] private int current_content_index;
    [BoxGroup("Content_Section")] [SerializeField] [ReadOnly] private List<string> content_detail;

    private void Awake()
    {
        if(ES3.KeyExists("Novel_List")==false)
        {
            novel_list = new List<novel_element>();
            ES3.Save("Novel_List", novel_list);
        }
        else
        {
            novel_list = ES3.Load<List<novel_element>>("Novel_List");
        }
    }

    private void save_novel_list()
    {
        ES3.Save("Novel_List", novel_list);
    }

    public void delete_current_element()
    {
        delete_element(current_title);
    }

    public void delete_current_chapter()
    {
        delete_chapter(current_chapter_title);
    }

    public void delete_current_content()
    {
        delete_content(current_content_title);
    }

    public void insert_element(string title, int code)
    {
        if(check_element(title)==-1)
        {
            novel_element tmp = new novel_element(title, code);
            novel_list.Add(tmp);
            save_novel_list();

            current_title = title;

            chapter_list.Clear();
            List<string> detail = new List<string>();
            detail.Add("작품의 설정을 담고 있습니다.");

            insert_chapter("Setting", detail);            
        }
    }

    public void insert_chapter(string name, List<string> detail)
    {
        if(check_chapter(name)==-1)
        {
            Debug.Log("insert Chapter : " + name);

            chapter_list.Add(name);
            ES3.Save(current_title, chapter_list);

            content_list.Clear();
            ES3.Save(ret_chapter_detail_key(current_title, name), detail);
        }
    }

    public void insert_content(string name, List<string> detail)
    {
        Debug.Log(ES3.KeyExists(ret_content_key(current_title, current_chapter_title, name))) ;

        if(current_content_index<content_list.Count)
        {
            if(ES3.KeyExists(ret_content_key(current_title,current_chapter_title,current_content_title))==true)
            {
                ES3.DeleteKey((ret_content_key(current_title, current_chapter_title, current_content_title))); // 이전 파일 삭제 추가
            }

            int number = 0;
            string original_name = name;
            while(true)
            {
                if(ES3.KeyExists(ret_content_key(current_title,current_chapter_title,name))==true)
                {
                    name = original_name + "_" + (++number).ToString();
                }
                else
                {
                    break;
                }
            }

            content_list[current_content_index] = name;
            ES3.Save(ret_chapter_key(current_title, current_chapter_title), content_list);
            ES3.Save(ret_content_key(current_title, current_chapter_title, name), detail);

        }
        else
        {
            int number = 0;
            string original_name = name;
            while (true)
            {
                if (ES3.KeyExists(ret_content_key(current_title, current_chapter_title, name)) == true)
                {
                    name = original_name + "_" + (++number).ToString();
                }
                else
                {
                    break;
                }
            }

            content_list.Add(name);
            ES3.Save(ret_chapter_key(current_title, current_chapter_title), content_list);
            ES3.Save(ret_content_key(current_title, current_chapter_title, name), detail);
        }
    }

    private void delete_element(string title)
    {
        int pos = check_element(title);
        if(pos!=-1)
        {
            List<string> chapters = ES3.Load<List<string>>(title);
            for(int i =0;i<chapters.Count;i++)
            {
                List<string> contents = ES3.Load<List<string>>(ret_chapter_key(title,chapters[i]));
                for(int j =0;j<contents.Count;j++)
                {
                    ES3.DeleteKey(ret_content_key(title, chapters[i], contents[j])); // Content Delete
                }
                ES3.DeleteKey(ret_chapter_key(title, chapters[i])); // Chapter Delete
                ES3.DeleteKey(ret_chapter_detail_key(title, chapters[i]));
            }
            ES3.DeleteKey(title);

            novel_list.RemoveAt(pos);
            save_novel_list();
        }
    }

    private void delete_chapter(string name)
    {
        int pos = check_chapter(name);
        if(pos!=-1)
        {
            if(ES3.KeyExists(ret_chapter_key(current_title, name)))
            {
                List<string> contents = ES3.Load<List<string>>(ret_chapter_key(current_title, name));
                for (int i = 0; i < contents.Count; i++)
                {
                    ES3.DeleteKey(ret_content_key(current_title, name, contents[i]));
                }

                ES3.DeleteKey(ret_chapter_key(current_title, name));
                ES3.DeleteKey(ret_chapter_detail_key(current_title, name));
            }

            chapter_list.RemoveAt(pos);
            ES3.Save(current_title, chapter_list);
        }
    }

    private void delete_content(string name)
    {
        int pos = check_content(name);
        if(pos!=-1)
        {
            if(ES3.KeyExists(ret_content_key(current_title, current_chapter_title, name)))
            {
                ES3.DeleteKey(ret_content_key(current_title, current_chapter_title, name));
            }

            content_list.RemoveAt(pos);
            ES3.Save(ret_chapter_key(current_title, current_chapter_title), content_list);
        }
    }

    private int check_element(string name)
    {
        for(int i =0;i<novel_list.Count;i++)
        {
            if(string.Compare(novel_list[i].novel_title,name)==0)
            {
                return i;
            }
        }
        return -1;
    }

    private int check_chapter(string name)
    {
        for(int i =0;i<chapter_list.Count;i++)
        {
            if(string.Compare(chapter_list[i], name)==0)
            {
                return i;
            }
        }
        return -1;
    }

    private int check_content(string name)
    {
        for(int i =0;i<content_list.Count;i++)
        {
            if(string.Compare(content_list[i],name)==0)
            {
                return i;
            }
        }
        return -1;
    }

    public void set_current_information(int index)
    {
        current_title = ret_index_of_title(index);
        current_code = novel_list[index].image_code;
        select_novel_event();

        chapter_list = ES3.Load<List<string>>(current_title);
    }

    public void set_current_chapter(string chapter_var)
    {
        current_chapter_title = chapter_var;
        current_chapter_detail = ES3.Load<List<string>>(ret_chapter_detail_key(current_title, current_chapter_title));
        select_chapter_event();

        if(ES3.KeyExists(ret_chapter_key(current_title, current_chapter_title)))
        {
            content_list = ES3.Load<List<string>>(ret_chapter_key(current_title, current_chapter_title));
        }
        else
        {
            content_list.Clear();
        }
    }

    public void set_current_content(string content_var, int index)
    {
        current_content_title = content_var;

        if(ES3.KeyExists(ret_content_key(current_title,current_chapter_title,current_content_title)))
        {
            content_detail = ES3.Load<List<string>>(ret_content_key(current_title, current_chapter_title, current_content_title));
        }
        else
        {
            content_detail.Clear();
        }
        current_content_index = index;

        select_content_event();
    }

    [Button]
    public void delete_all()
    {
        ES3.DeleteFile();
    }

    [Button]
    public void print_keys()
    {
        string[] key_value = ES3.GetKeys();
        for(int i =0;i<key_value.Length;i++)
        {
            Debug.Log(key_value[i] + "/");
        }
    }

    public void clear_current_content_detail()
    {
        content_detail.Clear();
    }

    public void set_current_content_index(int value)
    {
        current_content_index = value;
    }

    public string ret_current_title()
    {
        return current_title;
    }

    public string ret_current_chapter()
    {
        return current_chapter_title;
    }

    public string ret_current_content()
    {
        return current_content_title;
    }

    public List<string> ret_detail_of_chapter()
    {
        return ES3.Load<List<string>>(ret_chapter_detail_key(current_title, current_chapter_title));
    }

    public List<string> ret_detail_of_content()
    {
        return content_detail;
    }

    public Sprite ret_current_sprite()
    {
        return img_sprite[current_code];
    }

    public int ret_size_list()
    {
        return novel_list.Count;
    }

    public int ret_current_content_index()
    {
        return current_content_index;
    }

    public int ret_chapter_list()
    {
        return chapter_list.Count;
    }

    public int ret_content_list()
    {
        return content_list.Count;
    }

    public string ret_index_of_title(int index)
    {
        return novel_list[index].novel_title;
    }

    public string ret_name_of_chapter(int index)
    {
        return chapter_list[index];
    }

    public string ret_name_of_content(int index)
    {
        return content_list[index];
    }

    public string ret_chapter_key(string novel_name, string chapter_name)
    {
        return novel_name + "_" + chapter_name;
    }

    public string ret_chapter_detail_key(string novel_name, string chapter_name)
    {
        return novel_name + "_" + chapter_name + "(detail)";
    }

    public string ret_content_key(string novel_name,string chapter_name, string content_name)
    {
        return novel_name + "_" + chapter_name + "_" + content_name;
    }
}
