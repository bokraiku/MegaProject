using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

    private static DontDestroy GameObjectInstance;

    public GameObject PlayerSpwanPoint;
    public GameObject PlayerInventoryPoint;
    public GameObject PlayerPrefebs;
    void Awake()
    {
        if (!GameObjectInstance)
        {
            //DontDestroyOnLoad(transform.gameObject);
            GameObjectInstance = this;
        }
        else
        {
            Destroy(transform.gameObject);
        }


        //GameObject Player = (GameObject)Instantiate(PlayerPrefebs, PlayerSpwanPoint.transform.position, PlayerSpwanPoint.transform.rotation);
    }

     void Start()
    {
        //GameObject PlayerInventory = Instantiate(PlayerPrefebs,new Vector3(0, 0, 10), Quaternion.identity) as GameObject;

        //PlayerInventory.layer = 5;
        //PlayerInventory.name = "PlayerIntentory";
        //PlayerInventory.GetComponent<PlayerStatus>().enabled = false;
        //PlayerInventory.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().enabled = false;
        //PlayerInventory.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = false;
        //PlayerInventory.GetComponent<PlayerHealthManager>().enabled = false;
        //PlayerInventory.GetComponent<Rigidbody>().isKinematic = true;
        //PlayerInventory.GetComponent<Rigidbody>().useGravity = false;
        //PlayerInventory.GetComponent<CapsuleCollider>().enabled = false;
        //PlayerInventory.transform.parent =  PlayerInventoryPoint.transform;
    }
}
