using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterManager : LazySingleton<CharacterManager>
{
    #region :   Const

    private const string CharacterDataPath = "DataTable/Character_Table";
    private const string HeroStatusPath = "DataTable/Hero_Status";
    private const string MONSTER_STATUS_DATA_TABLE_PATH = "DataTable/Monster_Status";
    private const string CharacterPrefabPath = "SPUM/SPUM_Units";
    private const string CharacterImagePath = "CharacterImage";
    private const string CharacterTilePath = "CharacterImage/Tiles";
    private const int MonsterIndex = 2000000;

    #endregion

    public Dictionary<int, CharacterData> HeroDictionary { get { return heroDictionary; } }
    public Dictionary<int, CharacterData> MonsterDictionary { get { return monsterDictionary; } }

    public Dictionary<int, HeroStatusData> HeroStatusDictionary { get { return heroStatusDictionary; } }

    private Dictionary<int, CharacterData> heroDictionary = new Dictionary<int, CharacterData>();
    private Dictionary<int, CharacterData> monsterDictionary = new Dictionary<int, CharacterData>();
    private Dictionary<int, HeroStatusData> heroStatusDictionary = new Dictionary<int, HeroStatusData>();
    private Dictionary<int, MonsterStatusData> _monsterStatusDictionary = new Dictionary<int, MonsterStatusData>();

    CharacterManager()
    {
        LoadData();
    }

    public void LoadData()
    {
        //Character Table
        var data = DataTable.LoadJsonFile<TableData<CharacterData>>(CharacterDataPath);
        var dataArray = data.array;
        int count = dataArray.Count;
        for (int i = 0; i < count; i++)
        {
            int id = dataArray[i].Character_ID;
            if (id < MonsterIndex)
                heroDictionary.Add(id, dataArray[i]);
            else
                monsterDictionary.Add(id, dataArray[i]);
        }

        var stausData = DataTable.LoadJsonFile<TableData<HeroStatusData>>(HeroStatusPath);
        var statDataArray = stausData.array;
        count = statDataArray.Count;
        for (int i = 0; i < count; i++)
        {
            heroStatusDictionary.Add(statDataArray[i].Character_ID, statDataArray[i]);
        }

        var monsterStatusTableData = DataTable.LoadJsonFile<TableData<MonsterStatusData>>(MONSTER_STATUS_DATA_TABLE_PATH);
        var monsterStatusDatas = monsterStatusTableData.array;
        for (int i = 0, length = monsterStatusDatas.Count; i < length; ++i)
        {
            _monsterStatusDictionary.Add(monsterStatusDatas[i].Character_ID, monsterStatusDatas[i]);
        }
    }

    public CharacterData GetRandomHero()
    {
        int randValue = UnityEngine.Random.Range(0, heroDictionary.Count);
        List<int> keyList = new List<int>(heroDictionary.Keys);

        return heroDictionary[keyList[randValue]];
    }

    public Sprite GetCharacterSprite(CharacterData data)
    {
        if (data == null)
            return null;

        return Resources.Load<Sprite>($"{CharacterImagePath}/{data.Path_Img}");
    }

    public Tile GetCharacterTile(CharacterData data)
    {
        if (data == null)
            return null;

        return Resources.Load<Tile>($"{CharacterTilePath}/{data.Path_Img}");
    }

    public BaseHero GetHero(int characterID)
    {
        if (heroDictionary.ContainsKey(characterID) == false)
            return null;

        return Resources.Load<BaseHero>($"{CharacterPrefabPath}/{heroDictionary[characterID].Path_Units}");
    }

    public BaseMonster GetMonster(int characterID)
    {
        if (monsterDictionary.ContainsKey(characterID) == false)
            return null;

        return Resources.Load<BaseMonster>($"{CharacterPrefabPath}/{monsterDictionary[characterID].Path_Units}");
    }

    public HeroStatusData GetHeroStatusData(int characterID)
    {
        if (heroStatusDictionary.ContainsKey(characterID) == false)
            return null;

        return heroStatusDictionary[characterID];
    }

    public MonsterStatusData GetMonsterStatusData(int characterID)
    {
        if (_monsterStatusDictionary.ContainsKey(characterID) == false)
            return null;

        return _monsterStatusDictionary[characterID];
    }
}
