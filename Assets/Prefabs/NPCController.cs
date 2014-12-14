using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {

    public GameObject NPC = null;
    public GameObject BOSS = null;
    public GameObject[] Target;

    private GameObject[] bufNPC;
    private GameObject[] bufBOSS;

    public bool WinFlag = false;
    private bool ControllActiveFlag = true;
	// Use this for initialization
	void Start () {
        bufNPC = new GameObject[Target.Length];
        for (int i = 0; i < Target.Length; i++)
        {
            bufNPC[i] = (GameObject)Instantiate(NPC, Target[i].transform.position, Quaternion.identity);
            bufNPC[i].GetComponent<NavAgent>().Target = new GameObject[Target.Length];
            for (int j = 0; j < Target.Length; j++) bufNPC[i].GetComponent<NavAgent>().Target[j] = Target[j];
        }

        bufBOSS = new GameObject[Target.Length];
        for (int i = 0; i < Target.Length; i++)
        {
            Vector3 offset=new Vector3(5,0,5);
            bufBOSS[i] = (GameObject)Instantiate(BOSS, Target[i].transform.position+offset, Quaternion.identity);
            bufBOSS[i].GetComponent<NavAgent>().Target = new GameObject[Target.Length];
            for (int j = 0; j < Target.Length; j++) bufBOSS[i].GetComponent<NavAgent>().Target[j] = Target[j];
            bufBOSS[i].GetComponent<NavAgent>().randam = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        int winflag = 0;
        for (int i = 0; i < Target.Length; i++)
        {
            if(bufNPC[i].transform.FindChild("unitychan").GetComponent<UnityChanNPC>().LoseFlag==true)
            {
                winflag++;
            }
            if (bufBOSS[i].transform.FindChild("unitychan").GetComponent<UnityChanNPC>().LoseFlag == true)
            {
                winflag++;
            }
        }
        if (winflag >= Target.Length*2)
        {
            WinFlag = true;
        }
        if (ControllActiveFlag == true) SetControllActiveFlag(true);
	}
    public void SetControllActiveFlag(bool flag)
    {
        ControllActiveFlag = flag;
        if (ControllActiveFlag == true)
        {
            for (int i = 0; i < Target.Length; i++)
            {
                bufNPC[i].GetComponent<NavAgent>().NavStop();
                bufBOSS[i].GetComponent<NavAgent>().NavStop();
            }
        }
        else
        {
            for (int i = 0; i < Target.Length; i++)
            {
                bufNPC[i].GetComponent<NavAgent>().NavStart();
                bufBOSS[i].GetComponent<NavAgent>().NavStart();
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < Target.Length; i++)
        {
            bufNPC[i].transform.FindChild("unitychan").GetComponent<UnityChanNPC>().Reset();
            bufBOSS[i].transform.FindChild("unitychan").GetComponent<UnityChanNPC>().Reset();
        }
        WinFlag = false;
    }
}
