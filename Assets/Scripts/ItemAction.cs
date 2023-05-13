using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemAction : MonoBehaviour {

    public GameObject Effect_Prefab;
    public GameObject Lid;
    public float DangerDistance = 6.0f;
    public float moveSpeed = 1.5f;
    NavMeshAgent myNav;
    GameObject Player;

    void Start()     {
        Debug.Log("class ItemAction:Start()");
        Player = GameObject.FindGameObjectWithTag( "Player" );
        myNav = GetComponent<NavMeshAgent>();
        myNav.speed = moveSpeed;
    }

    void Vanish() {
        Debug.Log("class ItemAction:Vanish()");
        GameObject Fx = Instantiate( Effect_Prefab, transform.position + Vector3.up, Quaternion.identity );
        Destroy( Fx, 1.5f );
        Destroy( gameObject );
    }

     void Update() {
        if (myNav.enabled) {
            float D = Vector3.Distance( transform.position, Player.transform.position );
            if ( D <= DangerDistance ) {
                Vector3 Dir = transform.position - Player.transform.position;
                myNav.SetDestination( transform.position + Dir );
            }
        }
        if ( Lid ) {
            Lid.transform.localRotation =   Quaternion.Euler( ( ( Mathf.Cos( Time.time * Mathf.PI  * 3.0f ) + 1.0f ) / 2.0f ) * -45.0f, 0, 0 );
        }
    }


}
