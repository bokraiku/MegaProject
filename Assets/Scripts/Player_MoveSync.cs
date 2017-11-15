using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class Player_MoveSync : NetworkBehaviour  {

    [SyncVar(hook = "SyncPositionValues")]
    private Vector3 syncPos;

    [SerializeField]
    Transform myTransform;

    float lertRate = 15;
    private float normalLerpRate = 16;
    private float fasterLerpRate = 27;

    private Vector3 lastPos;
    private float threshold = 0.5f;

    private NetworkClient nClient;
    private int LatencyTime;
    private Text LatencyText;
    private List<Vector3> syncPosList = new List<Vector3>();
    [SerializeField]
    private bool useHistoricalLerping = false;

    private float closeEnough = 0.11f;
	// Use this for initialization

    void Start()
    {
        if (isLocalPlayer)
        {
            nClient = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>().client;
            LatencyText = GameObject.FindWithTag("LatencyText").GetComponent<Text>();
            lertRate = normalLerpRate;
        }
        
    }
    void Update()
    {
        ShowLatency();
    }
    void FixedUpdate()
    {
        TransmitPosition();

        LertPosition();
    }
    void LertPosition()
    {
        if (!isLocalPlayer)
        {
            if (useHistoricalLerping)
            {
                HistoricalLerping();
            }
            else
            {
                OrdinaryLerping();
            }
            
        }
    }
    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }
    [Client]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(myTransform.position,lastPos) > threshold)
        {
            CmdProvidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
        
    }
    [Client]
    void SyncPositionValues(Vector3 lastestPos)
    {
        syncPos = lastestPos;
        syncPosList.Add(syncPos);
    }
    void ShowLatency()
    {
        if (isLocalPlayer)
        {
            LatencyTime = nClient.GetRTT();
            LatencyText.text = LatencyTime.ToString();
        }
    }
    void OrdinaryLerping()
    {
        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.fixedDeltaTime * lertRate);
    }
    void HistoricalLerping()
    {
        if(syncPosList.Count > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.fixedDeltaTime * lertRate);
            if(Vector3.Distance(myTransform.position,syncPosList[0]) < closeEnough)
            {
                syncPosList.RemoveAt(0);
            }
            
        }
        if(syncPosList.Count > 10)
        {
            lertRate = fasterLerpRate;
        }
        else
        {
            lertRate = normalLerpRate;
        }
    }
}
