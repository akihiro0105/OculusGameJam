using UnityEngine;
using System.Collections;

public class UnityChanNPC : MonoBehaviour {

    public GameObject Particle;
    public bool LoseFlag = false;

    private Vector3 InitPos;
    private ParticleSystem P;
    // Use this for initialization
    void Start()
    {
        GetComponent<Animator>().SetFloat("Speed", 1);
        InitPos = transform.position;
        P = Particle.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.GetComponent<NavMeshAgent>().transform.position;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetFloat("Speed", 0);
            GetComponent<Animator>().SetBool("Lose", true);
            transform.parent.GetComponent<NavAgent>().NavStop();
            GetComponents<AudioSource>()[0].Stop();
            GetComponents<AudioSource>()[1].Stop();
            GetComponents<AudioSource>()[2].Play();
            LoseFlag = true;
            transform.parent.GetComponent<NavAgent>().StopFlag = true;
            P.Stop();
        }
    }

    public void Reset()
    {
        transform.parent.GetComponent<NavMeshAgent>().Warp(InitPos);
        GetComponent<Animator>().SetBool("Lose", false);
        GetComponent<Animator>().SetFloat("Speed", 1);
        GetComponents<AudioSource>()[0].Play();
        GetComponents<AudioSource>()[1].Play();
        GetComponents<AudioSource>()[2].Stop();
        LoseFlag = false;
        transform.parent.GetComponent<NavAgent>().StopFlag = false;
        P.Play();
    }
}
