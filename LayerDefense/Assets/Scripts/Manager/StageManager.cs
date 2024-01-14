using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : LazySingleton<StageManager>
{
    private const string STAGE_DATA_TABLE_PATH = "DataTable/Stage_Table";
    private const string STAGE_MONSTER_DATA_TABLE_PATH = "DataTable/Stage_Monster";

    public Dictionary<int, StageData> StageDictionary { get { return _stageDictionary; } }
    public Dictionary<int, StageMonsterData> StageMonsterDictionary { get { return _stageMonsterDictionary; } }

    private Dictionary<int, StageData> _stageDictionary = new Dictionary<int, StageData>();
    private Dictionary<int, StageMonsterData> _stageMonsterDictionary = new Dictionary<int, StageMonsterData>();
    private List<BaseMonster> _monsters = new List<BaseMonster>();
    private int _currentStageIndex = 0;
    private int _currentStageID = 0;
    private bool _isLoadedDataTables = false;

    StageManager()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (_isLoadedDataTables)
            return;

        var stageTableData = DataTable.LoadJsonFile<TableData<StageData>>(STAGE_DATA_TABLE_PATH);
        var stageDatas = stageTableData.array;
        for (int i = 0, length = stageDatas.Count; i < length; ++i)
        {
            _stageDictionary.Add(stageDatas[i].Stage_ID, stageDatas[i]);
        }
        _currentStageID = stageDatas[_currentStageIndex].Stage_ID;

        var stageMonsterTableData = DataTable.LoadJsonFile<TableData<StageMonsterData>>(STAGE_MONSTER_DATA_TABLE_PATH);
        var stageMonsterDatas = stageMonsterTableData.array;
        for (int i = 0, length = stageMonsterDatas.Count; i < length; ++i)
        {
            _stageMonsterDictionary.Add(stageMonsterDatas[i].Stage_ID, stageMonsterDatas[i]);
        }

        _isLoadedDataTables = true;
    }

    public void SetStageIndex(int stage)
    {
        _currentStageIndex = stage;
    }

    public bool TryGetStageData(out StageData stageData)
    {
        return _stageDictionary.TryGetValue(_currentStageID, out stageData);
    }

    public bool TryGetStageMonsterData(out StageMonsterData stageMonsterData)
    {
        return _stageMonsterDictionary.TryGetValue(_currentStageID, out stageMonsterData);
    }

    public StageMonsterData GetStageMonsterData(int stage, int wave)
    {
        var enumerator = _stageMonsterDictionary.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            var stageMonsterData = current.Value;
            if (stageMonsterData.Stage != stage)
                continue;

            if (stageMonsterData.Wave != wave)
                continue;

            return stageMonsterData;
        }

        return null;
    }

    public int GetStageMonsterTotalQuantity(int stage)
    {
        var total = 0;
        var enumerator = _stageMonsterDictionary.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            var stageMonsterData = current.Value;
            if (stageMonsterData.Stage != stage)
                continue;

            total += stageMonsterData.Monsterquantity;
        }

        return total;
    }

    public void AddMonster(BaseMonster monster)
    {
        _monsters.Add(monster);
    }

    public void RemoveMonster(BaseMonster monster)
    {
        _monsters.Remove(monster);
    }

    public BaseMonster GetMonster()
    {
        for (int i = 0, length = _monsters.Count; i < length; ++i)
        {
            var monster = _monsters[i];
            if (monster.IsDeath())
                continue;

            return monster;
        }

        return null;
    }
}
