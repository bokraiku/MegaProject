using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Net;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TestWWW : MonoBehaviour {
    private List<Item> database = new List<Item>();
    private JsonData itemData;
    WWWRequestor rq;
    public Text MSG;
    private WWW data;
    // Use this for initialization
    void Start () {
        rq = GetComponent<WWWRequestor>();
        MSG = GameObject.Find("MessagePanel").transform.GetChild(0).GetComponent<Text>();
        StartCoroutine("WaitForData");
    }

    IEnumerator WaitForData()
    {
        data = rq.GET();
        yield return new WaitForSeconds(1f);

        itemData = JsonMapper.ToObject(data.text);
        Debug.Log(itemData[0]["title"]);
        MSG.text = itemData[0]["title"].ToString();
        //ConstructInventoryManager();


    }
}
