using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;


public class Player_RotationSync : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerRotSynced")]
    private float syncPlayerRotation;
    [SyncVar]
    private Quaternion syncCamRotation;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform camTransform;

    float lertRate = 20;

    private float lastPlayerRot;
    private float theshold = 1;

    private List<float> syncPlayerRotList = new List<float>();
    private float closeEnough = 0.4f;
    [SerializeField] private bool UseHistoricRotation;
    // Use this for initialization

    void FixedUpdate()
    {
        TransmitRotation();
        LerpRotations();
    }
    void LerpRotations()
    {
        if (!isLocalPlayer)
        {
            if (UseHistoricRotation)
            {
                HistoricalInterRotation();
            }
            else
            {
                OridaryLerping();
            }
            //playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, syncPlayerRotation, Time.fixedDeltaTime * lertRate);
            //camTransform.rotation = Quaternion.Lerp(camTransform.rotation, syncCamRotation, Time.deltaTime * lertRate);
        }
    }

    void HistoricalInterRotation()
    {
        if(syncPlayerRotList.Count > 0)
        {
            LerpPlayerRotation(syncPlayerRotList[0]);
            if(Mathf.Abs(playerTransform.localEulerAngles.y - syncPlayerRotList[0]) < closeEnough)
            {
                syncPlayerRotList.RemoveAt(0);
            }
            
        }
        if (syncPlayerRotList.Count > 10)
        {

        }
    }

    void OridaryLerping()
    {
        LerpPlayerRotation(syncPlayerRotation);
    }
    void LerpPlayerRotation(float rotAngle)
    {
        Vector3 playerNewRot = new Vector3(0, rotAngle, 0);
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.Euler(playerNewRot), lertRate * Time.fixedDeltaTime);
    }
    [Command]
    void CmdProvideRotationToServer(float playerRot)
    {
        syncPlayerRotation  = playerRot;
        //syncCamRotation     = camRot;
    }

    bool CheckBeyondTheshould(float rot1,float rot2)
    {
        if(Mathf.Abs(rot1 - rot2) > theshold)
        {
            return true;
        }
        return false;
    }

    [Client]
    void TransmitRotation()
    {
        if (isLocalPlayer )
        {
            //if(Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > theshold)
            if(CheckBeyondTheshould(playerTransform.localEulerAngles.y,lastPlayerRot))
            {
                lastPlayerRot = playerTransform.localEulerAngles.y;

                CmdProvideRotationToServer(lastPlayerRot);
                
            }

        }

    }
    [Client]
    void OnPlayerRotSynced(float lastPlayerRot)
    {
        syncPlayerRotation = lastPlayerRot;
        syncPlayerRotList.Add(syncPlayerRotation);
    }
}
