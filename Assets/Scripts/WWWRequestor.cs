using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
class WWWRequestor : MonoBehaviour
{
    public string url = "https://th.winnerconnect.com/t_item.php";
    public WWW GET()
    {
        WWW www;
        
        www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    IEnumerator WaitForRequest(WWW data)
    {
        yield return data; // Wait until the download is done
        if (!String.IsNullOrEmpty(data.error))
        {
            Debug.Log("There was an error sending request: " + data.error);
        }
        else
        {
            Debug.Log("WWW Request: " + data.text);
        }
    }
}