using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを使う宣言
using UnityEngine.SceneManagement; //シーンマネージャーを使う宣言

public class ResultAction : MonoBehaviour {

    public Text[] txtRank = new Text[ 5 ];
    public Text txtResult;
    public GameObject Stage;
    public AudioClip SE_Result;
    AudioSource myAudio;

    void Start() {
        Debug.Log("class ResultAction:Start()");
        myAudio = GetComponent<AudioSource>(); //音源を取得
        myAudio.PlayOneShot( SE_Result ); //サウンド鳴動

        for ( int idx = 1 ; idx <= 5 ; idx++ ) {
            float StoredTime = PlayerPrefs.GetFloat( "Rank" + idx );
            if ( StoredTime == float.MaxValue ) {
                txtRank[ idx - 1 ].text = "=.==s";
            } else {
                txtRank[ idx - 1 ].text = StoredTime.ToString( "f2" ) + "s";
            }
        }
        txtResult.text = "Your Time " + PlayerPrefs.GetFloat( "Current" ).ToString( "f2" ) + "s"; //成績を表示
    }

    public void PushButton() {
        Debug.Log("class ResultAction:PushButton()");
        SceneManager.LoadScene( "Title" );
    }

    void Update() {
        if (Stage) {
            Stage.transform.Rotate( Vector3.up, 5 * Time.deltaTime );
        }

    }
}
