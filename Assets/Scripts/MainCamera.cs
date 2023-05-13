using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    GameObject P; //プレイヤー
    public Vector3 OffSet = new Vector3(0, 1.4f, 0); //彼女の口元付近
    public Vector3 CamDir = new Vector3(0, 4, -5.5f); //彼女から見たカメラ方向

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("class MainCamera:Start()");
        P = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!P)
        {
            return; //プレイヤー不在なら動かさない。
        }
        //カメラの位置。プレイヤー位置から算出する。
        transform.position = P.transform.position + CamDir;
        //カメラの回転。注視点はプレイヤーの少し上を見る。
        transform.LookAt(P.transform.position + OffSet);

    }
}
