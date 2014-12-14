using UnityEngine;
using System.Collections;

public class NPCController_target : MonoBehaviour {

    public int EnemyCount = 0;
    public GameObject Enemy = null;
    public GameObject Target;
    public GameObject[] Point;

    private GameObject[] bufEnemy;
    public bool LoseFlag = false;
    private bool ControllActiveFlag = true;
	// Use this for initialization
	void Start () {
        bufEnemy = new GameObject[EnemyCount];
        for (int i = 0; i < EnemyCount; i++)
        {
            bufEnemy[i] = (GameObject)Instantiate(Enemy, Point[Random.Range(0, Point.Length - 1)].transform.position, Quaternion.identity);
            bufEnemy[i].GetComponent<NavAgent>().Target = new GameObject[1];
            bufEnemy[i].GetComponent<NavAgent>().Target[0] = Target;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < EnemyCount; i++)
        {
            if (bufEnemy[i].transform.FindChild("UnityMask").GetComponent<UnityMaskNPC>().WinFlag == true)
            {
                LoseFlag = true;
                break;
            }
        }
        if (ControllActiveFlag == true) SetControllActiveFlag(true);
	}

    public void SetControllActiveFlag(bool flag)
    {
        ControllActiveFlag = flag;
        if (ControllActiveFlag == true)
        {
            for (int i = 0; i < EnemyCount; i++)
            {
                bufEnemy[i].GetComponent<NavAgent>().NavStop();
            }
        }
        else
        {
            for (int i = 0; i < EnemyCount; i++)
            {
                bufEnemy[i].GetComponent<NavAgent>().NavStart();
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < EnemyCount; i++)
        {
            bufEnemy[i].transform.FindChild("UnityMask").GetComponent<UnityMaskNPC>().Reset();
        }
        LoseFlag = false;
    }
}
