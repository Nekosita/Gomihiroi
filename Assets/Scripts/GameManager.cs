using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを利用するのに必要
using UnityEngine.SceneManagement; //シーンマネージャーを使う宣言

public class GameManager : MonoBehaviour {

    public enum MODE {
        PLAYING,
        TIMEUP,
        CLEAR
    }
    MODE GameMode; //ゲームモード

    public Text txtTime;
    public Button btnNavi;
    public Text txtNavi;
    public float LimitTime = 0.0f; //制限時間の初期値
    public Image imgMessage;
    public Text txtMessage;
    public GameObject Goal_Prefab;
    public GameObject Item_Prefab;
    public Image[] imgStar;
    public Sprite StarSprite;

    public AudioClip SE_Clear;
    public AudioClip SE_Get;
    public AudioClip SE_Over;
    public AudioClip Voice_Clear;
    public AudioClip Voice_Retry;
    public AudioClip Voide_Start;

    AudioSource myAudio;
    GameObject[] SpawnPos;
    GameObject Player;
    int MaxStar;
    float Elapsed; //経過時間
    int ItemCounter;
    float[] Rank = new float[ 6 ]; //順位管理の作業エリア
    List<Vector3> SpawnList;


    void Start() {
        Debug.Log("class GameManager:Start()");
        GameMode = MODE.PLAYING;
        myAudio = GetComponent<AudioSource>(); //音源を取得
        myAudio.PlayOneShot( Voide_Start ); //スタート鳴動
        Player = GameObject.FindGameObjectWithTag( "Player" ); //プレイヤーを取得
        TargetSpawn(); //ターゲット生成
        MaxStar = imgStar.Length;
        Elapsed = 0.0f;
        ItemCounter = 0;
        txtTime.text = "Time:0.00s";
        btnNavi.gameObject.SetActive( false );
        txtMessage.text = "START";
        imgMessage.gameObject.SetActive( true );
        Invoke( "EraseMessage", 2.0f );
    }


    void EraseMessage() {
        Debug.Log("class GameManager:EraseMessage()");
        imgMessage.gameObject.SetActive( false );
    }


    //ターゲット生成
    void TargetSpawn() {
        Debug.Log("class GameManager:TargetSpawn()");
        SpawnList = new List<Vector3>();
        SpawnPos = GameObject.FindGameObjectsWithTag( "Spawn" );
        for ( int idx = 0 ; idx < SpawnPos.Length ; idx++ ) {
            SpawnList.Add( SpawnPos[ idx ].transform.position );
        }
        for ( int idx = 0 ; idx < SpawnPos.Length ; idx++ ) {
            int Ran = Random.Range( 0, SpawnPos.Length );
            Vector3 Esc = SpawnList[ idx ];
            SpawnList[ idx ] = SpawnList[ Ran ];
            SpawnList[ Ran ] = Esc;
        }
        for ( int idx = 0 ; idx < imgStar.Length ; idx++ ) {
            Instantiate( Item_Prefab, SpawnList[ idx ], Quaternion.identity );
        }
    }


    public void PushNavi() {
        Debug.Log("class GameManager:PushNavi()");
        switch ( GameMode ) {
            case MODE.TIMEUP:
                SceneManager.LoadScene( "Title" );
                break;
            case MODE.CLEAR:
                SceneManager.LoadScene( "Result" );
                break;
            default:
                break;
        }
    }


    //アイテム獲得受信
    void VanishItem() {
        Debug.Log("class GameManager:VanishItem()");
        ItemCounter++;
        myAudio.PlayOneShot( SE_Get ); //獲得サウンド鳴動
        imgStar[ ItemCounter - 1 ].GetComponent<Image>().sprite = StarSprite;
        if ( MaxStar == ItemCounter ) {
            int Ran = Random.Range( 0, MaxStar );
            Instantiate( Goal_Prefab, SpawnList[ Ran ], Quaternion.identity );
        }
    }


    //ランキング処理
    void Ranking() {
        Debug.Log("class GameManager:Ranking()");
        for ( int idx = 1 ; idx <= 5 ; idx++ ) {
            Rank[ idx ] = PlayerPrefs.GetFloat( "Rank" + idx );
        }
        int newRank = 0; //まず今回のタイムを0位と仮定する
        for ( int idx = 5 ; idx > 0 ; idx-- ) { //逆順 5...1
            if ( Rank[ idx ] > Elapsed ) {
                newRank = idx; //着位ランク発見
            }
        }
        if ( newRank != 0 ) { //0位のままでなかったらランクイン確定
            for ( int idx = 5 ; idx > newRank ; idx-- ) {
                Rank[ idx ] = Rank[ idx - 1 ]; //繰り下げ処理
            }
            Rank[ newRank ] = Elapsed; //新ランクに登録
            for ( int idx = 1 ; idx <= 5 ; idx++ ) {
                PlayerPrefs.SetFloat( "Rank" + idx, Rank[ idx ] ); //データ領域に保存
            }
        }
    }

    //ゲームクリア受信
    void GameClear() {
        Debug.Log("class GameManager:GameClear()");
        GameMode = MODE.CLEAR;
        imgMessage.gameObject.SetActive( true );
        txtMessage.text = "Game Clear !";
        btnNavi.gameObject.SetActive( true );
        txtNavi.text = "RESULT";
        Ranking(); //ランキング
        PlayerPrefs.SetFloat( "Current", Elapsed );
        myAudio.Stop();
        myAudio.PlayOneShot( SE_Get ); //獲得サウンド鳴動
        myAudio.PlayOneShot( SE_Clear ); //クリア鳴動
        StartCoroutine( LateAudio( 1.8f, Voice_Clear ) ); //ボイスは遅延鳴動
    }



    //遅延鳴動処理
    IEnumerator LateAudio( float Delay , AudioClip Clip ) {
        Debug.Log("class GameManager:IEnumerator LateAudio()");
        yield return new WaitForSeconds( Delay ); //遅延
        myAudio.PlayOneShot( Clip ); //鳴動
    }


    void Update() {
        if ( GameMode == MODE.PLAYING ) {
            Elapsed += Time.deltaTime;
            if ( LimitTime - Elapsed < 0.0f ) {
                GameMode = MODE.TIMEUP; //タイムアップ
                Player.SendMessage( "GameFinish", SendMessageOptions.DontRequireReceiver );
                imgMessage.gameObject.SetActive( true );
                txtMessage.text = "Time Up !";
                txtTime.text = "Time:" + LimitTime.ToString("f2") + "s";
                txtNavi.text = "TITLE";
                btnNavi.gameObject.SetActive( true );
                myAudio.Stop();
                myAudio.PlayOneShot( SE_Over ); //タイムアップ鳴動
                StartCoroutine( LateAudio( 2.2f , Voice_Retry ) ); //ボイスは遅延鳴動
            } else {
                txtTime.text = "Time:" + Elapsed.ToString( "f2" ) + "s";
            }
        }
    }
}
