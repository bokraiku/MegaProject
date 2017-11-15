using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Net;
using UnityEngine.UI;
using UnityEngine.Networking;
using SocketIO;


public class SocketController : MonoBehaviour {

    private SocketIOComponent socket;

    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
    }
	

    public void UnEquipment(int role_id,int item_id,int inv_id,int updateItem)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["role_id"] = role_id.ToString();
        data["item_id"] = item_id.ToString();
        data["inv_id"] = inv_id.ToString();
        data["update_item"] = updateItem.ToString();
        socket.Emit("up_equipment",new JSONObject(data));
    }


    public void Equipment(int role_id, int item_id, int inv_id, int updateItem)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["role_id"] = role_id.ToString();
        data["item_id"] = item_id.ToString();
        data["inv_id"] = inv_id.ToString();
        data["update_item"] = updateItem.ToString();
        socket.Emit("equipment", new JSONObject(data));
    }

    public void GetPlayerAttr(int role_id)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["role_id"] = role_id.ToString();
        socket.Emit("equipment", new JSONObject(data));
    }

}
