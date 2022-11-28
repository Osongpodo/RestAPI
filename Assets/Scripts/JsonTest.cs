using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JsonTest : MonoBehaviour
{
    public delegate void callback(string str);

    public void LoadData(callback func)
    {
        StartCoroutine(UnityWebRequestGETTest(func));
    }

    IEnumerator UnityWebRequestGETTest(callback func)
    {
        string url = "http://106.247.250.251:31866/read_ints";

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            func(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }
}
