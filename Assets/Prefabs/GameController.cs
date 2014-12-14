using UnityEngine;
using System.Collections;
using UnityEngine.UI;//Text参照に必要

public class GameController : MonoBehaviour {

    public GameObject Player;
    public GameObject NPC;
    public AudioClip StartVoice;
    public AudioClip GameOverVoice;
    public AudioClip GameClearVoice;
    public GameObject Text;

    private UnityChan_AndroidControll P;
    private NPCController UnityChan;
    private NPCController_target UnityMask;
    private AudioSource Audio;
    private bool InitFlag = false;
    private bool StartFlag = false;
    private bool AudioFlag = false;
    private Text TextUI;
	// Use this for initialization
	void Start () {
        Audio = GetComponent<AudioSource>();
        Audio.clip = StartVoice;
	}

    void Init()
    {
        P = Player.GetComponent<UnityChan_AndroidControll>();
        UnityChan = NPC.GetComponent<NPCController>();
        UnityMask = NPC.GetComponent<NPCController_target>();
        InitFlag = true;
        TextUI=Text.GetComponent<Text>();
        TextUI.text = "クリックでスタート";
        TextUI.color = Color.white;
    }
	// Update is called once per frame
	void Update () {
        if (InitFlag == false) Init();
        else
        {
            if (StartFlag == false) GameStart();
            else GameEnd();
        }
	}

    void GameStart()
    {
        if (Input.GetButtonUp("Fire1"))//ゲームスタート
        {
            Audio.clip = StartVoice;
            Audio.Play();
            AudioFlag = true;
            TextUI.text = "3、2、1、スタート！";
            TextUI.color = Color.white;
        }
        if (AudioFlag == true)
        {
            if (Audio.isPlaying == false)
            {
                StartFlag = true;
                P.SetControllActiveFlag(false);
                UnityChan.SetControllActiveFlag(false);
                UnityMask.SetControllActiveFlag(false);
                AudioFlag = false;
                TextUI.text = "";
            }
        }
    }
    void GameEnd()
    {
        if (UnityChan.WinFlag == true)//ゲームクリア
        {
            if (AudioFlag == false)
            {
                Audio.clip = GameClearVoice;
                Audio.Play();
                P.SetControllActiveFlag(true);
                UnityChan.SetControllActiveFlag(true);
                UnityMask.SetControllActiveFlag(true);
                AudioFlag = true;
                TextUI.text = "　ゲーム　クリア！";
                TextUI.color = Color.blue;
            }
            else
            {
                if (Audio.isPlaying == false) if (Input.GetButtonUp("Fire1")) Reset();
            }
        }
        if (UnityMask.LoseFlag == true)//ゲームオーバー
        {
            if (AudioFlag == false)
            {
                Audio.clip = GameOverVoice;
                Audio.Play();
                P.SetControllActiveFlag(true);
                UnityChan.SetControllActiveFlag(true);
                UnityMask.SetControllActiveFlag(true);
                AudioFlag = true;
                TextUI.text = "　ゲーム　オーバー";
                TextUI.color = Color.red;
            }
            else
            {
                if (Audio.isPlaying == false) if (Input.GetButtonUp("Fire1")) Reset();
            }
        }
    }
    void Reset()
    {
        P.Reset();
        UnityChan.Reset();
        UnityMask.Reset();
        StartFlag = false;
        AudioFlag = false;
        TextUI.text = "クリックでスタート";
        TextUI.color = Color.white;
    }
}