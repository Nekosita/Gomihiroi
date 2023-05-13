using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleAction : MonoBehaviour {

    public AudioClip SE_Title;
    AudioSource myAudio;

    void Start() {
        myAudio = GetComponent<AudioSource>(); //音源を取得
        myAudio.PlayOneShot( SE_Title ); //サウンド鳴動
        if ( !PlayerPrefs.HasKey( "Rank1" ) ) {
            for ( int idx = 1 ; idx <= 5 ; idx++ ) {
                PlayerPrefs.SetFloat( "Rank" + idx, float.MaxValue );
            }
        }
    }

    public void PushStart() {
        SceneManager.LoadScene("Stage1");
    }

    void Update() {
        if ( Input.GetKeyDown( KeyCode.Escape ) ) {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene( gameObject.scene.name );
        }
    }
}
