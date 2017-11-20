using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Monster_MotionSync : NetworkBehaviour {

    [SyncVar] private Vector3 syncPos;
    [SyncVar] private float syncYRot;

    private Vector3 lastPos;
    private Quaternion lastRot;
    private Transform myTransform;
    private float LerpRate = 10f;
    private float posTheshold = 0.5f;
    private float rotTheshold = 5.0f;
	// Use this for initialization
	void Start () {
        myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        TransmitMotion();
        LertMotion();
	}

    void TransmitMotion()
    {
        if (!isServer)
        {
            return;
        }
        if(Vector3.Distance(myTransform.position,lastPos) > posTheshold || Quaternion.Angle(myTransform.rotation,lastRot) > rotTheshold)
        {
            lastPos = myTransform.position;
            lastRot = myTransform.rotation;

            syncPos = myTransform.position;
            syncYRot = myTransform.localEulerAngles.y;
        }
    }
    void LertMotion()
    {
        if (isServer)
        {
            return;
        }
        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * LerpRate);
        Vector3 newRot = new Vector3(0, syncYRot, 0);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation,Quaternion.Euler(newRot),Time.deltaTime * LerpRate);
    }
}
