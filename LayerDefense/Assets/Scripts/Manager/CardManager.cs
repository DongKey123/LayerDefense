using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardManager : LazySingleton<CardManager>
{
    private const string CardDataPath = "DataTable/Card_Table";


    public readonly int StartCardID = 10000;

    public Dictionary<int, CardData> CardDictionary
    {
        get { return cardDictionary; }
    }
    private Dictionary<int, CardData> cardDictionary = new Dictionary<int, CardData>();

    private CardManager()
    {
        LoadData();
    }

    public void LoadData()
    {
        var data = DataTable.LoadJsonFile<TableData<CardData>>(CardDataPath);

        int count = data.array.Count;
        var dataArray = data.array;

        for (int i = 0; i < count; i++)
        {
            cardDictionary.Add(dataArray[i].iD, dataArray[i]);
        }
    }

    public int GetRandomCardKey()
    {
        //Todo: Get Random Access
        int randomKey;

        randomKey = UnityEngine.Random.Range(StartCardID + 1, StartCardID + cardDictionary.Count);
        return randomKey;
    }
}
