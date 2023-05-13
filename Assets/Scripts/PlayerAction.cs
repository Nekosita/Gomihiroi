using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput; //プラットフォーム互換入力に必要

public class PlayerAction : MonoBehaviour {

    float h; //水平軸
    float v; //垂直軸
    Vector3 Dir; //移動方向
    Animator myAnim; //自身のアニメーター
    float movSpeed = 4.0f; //前進速度
    GameObject Manager; //マネージャー

    void Start() {
        Debug.Log("class PlayerAction:Start()");
        myAnim = GetComponent<Animator>(); //自身のアニメーターを取得
        Manager = GameObject.FindGameObjectWithTag( "MainCamera" );
    }

    void OnTriggerEnter( Collider other ) {
        Debug.Log("class PlayerAction:OnTriggerEnter()");
        if ( other.gameObject.tag == "Item") {
            other.gameObject.SendMessage( "Vanish", SendMessageOptions.DontRequireReceiver );
            Manager.SendMessage( "VanishItem", SendMessageOptions.DontRequireReceiver );
        }
        if ( other.gameObject.tag == "Finish" ) {
            other.gameObject.SendMessage( "Vanish", SendMessageOptions.DontRequireReceiver );
            Manager.SendMessage( "GameClear", SendMessageOptions.DontRequireReceiver );
            GameFinish();
        }
    }

    void GameFinish() {
        Debug.Log("class PlayerAction:GameFinish()");
        enabled = false;
        myAnim.SetFloat( "Speed", 0 );
    }

    void FixedUpdate() {
        h = CrossPlatformInputManager.GetAxis( "Horizontal" ); //仮想スティックの水平軸
        v = CrossPlatformInputManager.GetAxis( "Vertical" ); //仮想スティックの垂直軸
        Dir = new Vector3( h, 0, v );
        myAnim.SetFloat( "Speed", Dir.magnitude );
        transform.position += movSpeed * Dir * Time.fixedDeltaTime; ; //キャラクターを移動
        //キャラクターを回転
        Vector3 LookDir = transform.position + Dir;
        LookDir = Vector3.Slerp( transform.position + transform.forward, LookDir, 10.0f * Time.fixedDeltaTime );
        transform.LookAt( LookDir );
    }
}
