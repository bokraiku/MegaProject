using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManagement : MonoBehaviour {

    public GameObject[] worldZone;
    public GameObject[] worldObject;

    public GameObject[] warpZone;
    public GameObject[] spawnPoint;

    private Dictionary<int, int> WarpID = new Dictionary<int, int>();


    public GameObject LocalPlayerCurrentZone;
    //public GameObject LocalPlayerEnterWarp;

    public static GameManagement instane;

	// Use this for initialization
	void Awake () {
		if(instane == null)
        {
            instane = this;
        }
	}
    void Start()
    {
        this.WarpID[0] = 1;
        this.WarpID[1] = 0;

    }
	
	// Update is called once per frame
	void Update () {

        
	}
    public GameObject ManageWarp(string currentWarp)
    {
        GameObject warpPosition = null;
        if (warpZone.Length > 0)
        {
            
            for (int i = 0; i < warpZone.Length; i++)
            {
               
                if (warpZone[i].name == currentWarp)
                {
                    Debug.Log("warp : " + warpZone[i].name);
                    warpPosition = spawnPoint[WarpID[i]];
                    break;
                    
                }
                
            }
        }
        return warpPosition;


    }

    public void DeActivateZone()
    {
        if (GameManagement.instane.LocalPlayerCurrentZone != null)
        {
            //Debug.Log("Game Manager : " + GameManagement.instane.LocalPlayerCurrentZone.name);
            for (int i = 0; i < worldZone.Length; i++)
            {

                if (worldZone[i].name != GameManagement.instane.LocalPlayerCurrentZone.name)
                {
                    worldObject[i].SetActive(false);
                }
                else
                {
                    worldObject[i].SetActive(true);
                }
            }
        }
    }
}
