using UnityEngine;
using System.Collections;

public class NavAgent : MonoBehaviour {
    public GameObject[] Target;
    public bool randam = false;
    public float distance = 1.0f;

    private NavMeshAgent agent;
    private int TargetNum = 0;
    private int NavNum = 0;
    public bool StopFlag = false;
	// Use this for initialization
	void Start () {
        TargetNum = Target.Length-1;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Target[SetNavNum()].transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        if (agent.remainingDistance < distance && StopFlag==false)
        {
            agent.SetDestination(Target[SetNavNum()].transform.position);
        }
	}

    private int SetNavNum()
    {
        if (randam == true)
        {
            return Random.Range(0, TargetNum);
        }
        else
        {
            if (NavNum > TargetNum-1) NavNum=0;
            else NavNum++;
            return NavNum;
        }
    }

    public void NavStop()
    {
        agent.Stop();
    }

    public void NavStart()
    {
        agent.SetDestination(Target[SetNavNum()].transform.position);
    }
}
