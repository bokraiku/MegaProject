using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Monsterworld_ID : NetworkAnimator {

    [SyncVar]
    public string _monsterworld_id;
    private Transform myTransform;

	// Use this for initialization
	void Start () {
        myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        SetIdentity();
	}

    void SetIdentity()
    {
        if(myTransform.name == "" || myTransform.name == "Monster1(Clone)")
        {
            myTransform.name = _monsterworld_id; 
        }
    }
}
