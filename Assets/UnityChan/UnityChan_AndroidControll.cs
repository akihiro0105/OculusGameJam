using UnityEngine;
using System.Collections;

public class UnityChan_AndroidControll : MonoBehaviour {

    //Android
    public float androidY = 0;
    public float androidZ = 0;
    private bool ControllActiveFlag = true;
    private Vector3 InitPos;


    public float animSpeed = 1.5f;				// アニメーション再生速度設定
    public float lookSmoother = 3.0f;			// a smoothing setting for camera motion
    public bool useCurves = true;				// Mecanimでカーブ調整を使うか設定する
    // このスイッチが入っていないとカーブは使われない
    public float useCurvesHeight = 0.5f;		// カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

    // 以下キャラクターコントローラ用パラメタ
    // 前進速度
    public float forwardSpeed = 7.0f;
    // 後退速度
    public float backwardSpeed = 2.0f;
    // 旋回速度
    public float rotateSpeed = 2.0f;
    // キャラクターコントローラ（カプセルコライダ）の参照
    private CapsuleCollider col;
    private Rigidbody rb;
    // キャラクターコントローラ（カプセルコライダ）の移動量
    private Vector3 velocity;
    // CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
    private float orgColHight;
    private Vector3 orgVectColCenter;

    private Animator anim;							// キャラにアタッチされるアニメーターへの参照
    private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

    // アニメーター各ステートへの参照
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");

    // 初期化
    void Start()
    {
        // Animatorコンポーネントを取得する
        anim = GetComponent<Animator>();
        // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
        orgColHight = col.height;
        orgVectColCenter = col.center;
        //
        InitPos = transform.position;
    }


    // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
    void FixedUpdate()
    {
        if (ControllActiveFlag == true) return;
        float h = Input.GetAxis("Horizontal");				// 入力デバイスの水平軸をhで定義
        float v = Input.GetAxis("Vertical");				// 入力デバイスの垂直軸をvで定義
        v += androidY;
        if (v > 1) v = 1;
        else if (v < -1) v = -1;
        h += androidZ;
        if (h > 1) h = 1;
        else if (h < -1) h = -1;
        anim.SetFloat("Speed", v);							// Animator側で設定している"Speed"パラメタにvを渡す
        anim.SetFloat("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す
        anim.speed = animSpeed;								// Animatorのモーション再生速度に animSpeedを設定する
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする



        // 以下、キャラクターの移動処理
        velocity = new Vector3(0, 0, v);		// 上下のキー入力からZ軸方向の移動量を取得
        // キャラクターのローカル空間での方向に変換
        velocity = transform.TransformDirection(velocity);
        //以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
        if (v > 0.1)
        {
            velocity *= forwardSpeed;		// 移動速度を掛ける
        }
        else if (v < -0.1)
        {
            velocity *= backwardSpeed;	// 移動速度を掛ける
        }

        // 上下のキー入力でキャラクターを移動させる
        transform.localPosition += velocity * Time.fixedDeltaTime;

        // 左右のキー入力でキャラクタをY軸で旋回させる
        transform.Rotate(0, h * rotateSpeed, 0);


        // 以下、Animatorの各ステート中での処理
        // Locomotion中
        // 現在のベースレイヤーがlocoStateの時
        if (currentBaseState.nameHash == locoState)
        {
            //カーブでコライダ調整をしている時は、念のためにリセットする
            if (useCurves)
            {
                resetCollider();
            }
        }
        // IDLE中の処理
        // 現在のベースレイヤーがidleStateの時
        else if (currentBaseState.nameHash == idleState)
        {
            //カーブでコライダ調整をしている時は、念のためにリセットする
            if (useCurves)
            {
                resetCollider();
            }
        }
    }

    // キャラクターのコライダーサイズのリセット関数
    void resetCollider()
    {
        // コンポーネントのHeight、Centerの初期値を戻す
        col.height = orgColHight;
        col.center = orgVectColCenter;
    }

    public void SetControllActiveFlag(bool flag)
    {
        ControllActiveFlag = flag;
    }

    public void Reset()
    {
        transform.position = InitPos;
    }
}
