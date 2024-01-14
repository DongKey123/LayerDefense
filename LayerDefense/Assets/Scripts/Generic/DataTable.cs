using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTable
{
    public static T LoadJsonFile<T>(string path)
    {
        TextAsset json = Resources.Load<TextAsset>(path);

        return JsonConvert.DeserializeObject<T>(json.text);
    }
}

[Serializable]
public class TableData<T>
{
    public string md5;
    public List<T> array;
}