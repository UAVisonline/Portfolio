using UnityEngine;
using System.Collections;

public class trap_making : MonoBehaviour {

    public GameObject trap1, trap2, trap3, trap4;
    private Vector3 dir;
    private int[] num = new int[4];
	// Use this for initialization
	void Start () {
        num[0] = Random.Range(0, 4);
        for(int i=1;i<=3;i++)
        {
            int trap_num = Random.Range(0, 4);
            for(int j=0;j< i;j++)
            {
                if(trap_num == num[j])
                {
                    trap_num = Random.Range(0, 4);
                    j = -1;
                }
            }
            num[i] = trap_num;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    for(int i=0;i<=3;i++)
        {
            switch (i)
            {
                case 0 : dir = this.transform.position + new Vector3(-1.00f, -1.00f, 0.0f);
                    break;
                case 1: dir = this.transform.position + new Vector3(1.00f, -1.00f, 0.0f);
                    break;
                case 2: dir = this.transform.position + new Vector3(-1.00f, 1.00f, 0.0f);
                    break;
                case 3: dir = this.transform.position + new Vector3(1.00f, 1.00f, 0.0f);
                    break;
                default:
                    break;
            }   
            switch (num[i])
            {
                case 0:
                    Instantiate(trap1,dir, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(trap2, dir, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(trap3, dir, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(trap4, dir, Quaternion.identity);
                    break;
                default:
                    break;
            }
        }
        Destroy(gameObject);
	}
}
