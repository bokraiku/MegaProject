using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_ID : NetworkBehaviour {

    [SyncVar]
    public string playerUniqueName;
    private NetworkInstanceId playerNetID;
    private Transform myTransform;
    public override void OnStartLocalPlayer()
    {
      
        GetNetidentity();
        Setidentity();
    }
    // Use this for initialization
    void Awake () {
        myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(myTransform.name == "" || myTransform.name == "Player(Clone)")
        {
            Setidentity();

        }
	}

    [Client]
    void GetNetidentity()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }

    [Client]
    void Setidentity()
    {
        if (!isLocalPlayer)
        {
            myTransform.name = playerUniqueName;
        }
        else
        {
            myTransform.name = MakeUniqueIdentity();
        }
    }


    string MakeUniqueIdentity()
    {
        string uniqueName = "Player " + playerNetID.ToString();
        Debug.Log("Unique Name : " + uniqueName);
        return uniqueName;
    }


    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueName = name;
    }



}
