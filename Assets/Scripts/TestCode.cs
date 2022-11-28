using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public JsonTest test;

    void Start()
    {
        test.LoadData(printLog);

    }

    public void printLog(string str)
    {
        Value vv = JsonUtility.FromJson<Value>(str);

        foreach(Data data in vv.int_table)
        {
            data.printData();
            Debug.Log("====================================");
        }
    }
}

[Serializable]
public class Data
{
    public int id;
    public string logging_time;
    public int int_value;

    public void printData()
    {
        Debug.Log("id : " + id);
        Debug.Log("logging_time : " + logging_time);
        Debug.Log("int_value : " + int_value);
    }
}

[Serializable]
public class Value
{
    public Data[] int_table;
}
