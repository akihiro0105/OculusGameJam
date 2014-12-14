using UnityEngine;
using System.Collections;

public class UnityMaskNPC : MonoBehaviour {

    public bool WinFlag = false;

    private Vector3 InitPos;
	// Use this for initialization
	void Start () 
    {
        GetComponent<Animator>().SetFloat("Speed", 1);
        InitPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = transform.parent.GetComponent<NavMeshAgent>().transform.position;
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetFloat("Speed", 0);
            GetComponent<Animator>().SetBool("Win", true);
            transform.parent.GetComponent<NavAgent>().NavStop();
            GetComponents<AudioSource>()[0].Stop();
            WinFlag = true;
            transform.parent.GetComponent<NavAgent>().StopFlag = true;
        }
    }
    public void Reset()
    {
        transform.parent.GetComponent<NavMeshAgent>().Warp(InitPos);
        GetComponent<Animator>().SetBool("Win", false);
        GetComponent<Animator>().SetFloat("Speed", 1);
        GetComponents<AudioSource>()[0].Play();
        WinFlag = false;
        transform.parent.GetComponent<NavAgent>().StopFlag = false;
    }
}
