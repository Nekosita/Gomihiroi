using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCameraAction : MonoBehaviour {

    GameObject P;

    void Start() {
        P = GameObject.FindGameObjectWithTag( "Player" );
    }

    void Update() {
        transform.position = P.transform.position + Vector3.up * 50;
    }
}
