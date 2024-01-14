using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVESceneController : BaseSceneController, IActor
{
    #region :   ReadOnly

    private readonly string HOME_SCENE_NAME = "HomeScene";

    #endregion

    #region :   Inspector

    [SerializeField] CameraController _cameraController;
    [SerializeField] float _shakeScale;
    [SerializeField] float _shakeTime;

    #endregion

    #region :   Cached Manager

    private StageManager _stageManager = null;
    private CharacterManager _characterManager = null;
    private InGameDataManager _inGameDataManager = null;
    private CardManager _cardManager = null;
    private UIManager _uiManager = null;

    #endregion

    #region Card
    private Vector3Int worldCentPos = new Vector3Int(0, -2, 0);
    private Card curSelectedCard = null;
    private bool isPickCard = false;
    private bool isPossibleSetCard = false;
    private CardSetController cardSetController = null;
    #endregion

    #region Income
    private float incomeInterval = 1f;
    private float curIncomeTime = 0f;
    private int incomePerTime = 0;
    #endregion

    public bool IsGameRuning { get { return isGameRuning; } }

    private Dictionary<int, List<BaseMonster>> _waveMonsterDictionary = new Dictionary<int, List<BaseMonster>>();
    private MapController _mapController = null;
    private BehaviourTree _behaviourTree = null;
    private PVEGamePanelController inGamePanel = null;
    private float _currentTime = 0;
    private bool isGameRuning = false;

    public bool IsGameStart()
    {
        return isGameRuning;
    }

    public bool IsClearStage()
    {
        if (_inGameDataManager.HP <= 0)
            return false;

        var currentMonsterQuantity = GetCurrentMonsterQuantity();
        var monsterTotalQuantity = _inGameDataManager.currentMonsterTotalQuantity;
        if (currentMonsterQuantity != monsterTotalQuantity)
            return false;

        var monsterKilledTotalQuantity = _inGameDataManager.currentMonsterKilledTotalQuantity;
        if (monsterKilledTotalQuantity == monsterTotalQuantity)
            return true;

        return IsAllMonsterGoal();
    }

    public bool IsFailStage()
    {
        return _inGameDataManager.HP <= 0;
    }

    public bool IsFullWaveMonster()
    {
        var stage = _inGameDataManager.currentStage;
        var wave = _inGameDataManager.currentWave;
        var stageMonsterData = _stageManager.GetStageMonsterData(stage, wave);
        if (stageMonsterData == null)
            return false;

        if (_waveMonsterDictionary.TryGetValue(wave, out var monsters) == false)
            return false;

        if (monsters == null)
            return false;

        return monsters.Count == stageMonsterData.Monsterquantity;
    }

    public bool IsNextWave()
    {
        var stage = _inGameDataManager.currentStage;
        var wave = _inGameDataManager.currentWave;
        var stageMonsterData = _stageManager.GetStageMonsterData(stage, wave + 1);

        return stageMonsterData != null;
    }

    public bool IsDelayedCreateMonster()
    {
        var stage = _inGameDataManager.currentStage;
        var wave = _inGameDataManager.currentWave;
        var stageMonsterData = _stageManager.GetStageMonsterData(stage, wave);
        if (stageMonsterData == null)
            return false;

        _currentTime += Time.deltaTime;
        if (_currentTime < stageMonsterData.Monstergeneration)
            return false;

        _currentTime = 0;
        return true;
    }

    public void CreateMonster()
    {
        var stage = _inGameDataManager.currentStage;
        var wave = _inGameDataManager.currentWave;
        var stageMonsterData = _stageManager.GetStageMonsterData(stage, wave);
        if (stageMonsterData == null)
            return;

        OnCreateMonster(stageMonsterData);
    }

    public void ClearStage()
    {
        isGameRuning = false;

        var PANEL_PATH = "Game/GameSuccessPanel";
        _uiManager.Show<GameSuccessPanelController>(PANEL_PATH);
    }

    public void FailStage()
    {
        isGameRuning = false;

        var PANEL_PATH = "Game/GameFailPanel";
        _uiManager.Show<GameFailPanelController>(PANEL_PATH);
    }

    #region :   Unity Message

    protected override void Awake()
    {
        base.Awake();

        BaseMonster.OnGoal += OnGoal;
        BaseMonster.OnDeath += OnDeath;

        _behaviourTree = this.GetComponent<BehaviourTree>();

        _stageManager = StageManager.getInstance;
        _characterManager = CharacterManager.getInstance;
        _inGameDataManager = InGameDataManager.getInstance;
        _cardManager = CardManager.getInstance;
        _uiManager = UIManager.getInstance;

        InitializeGameData();
        InitializeUI();
        InitializeMap();
    }

    private void Start()
    {
        GameStart();
    }

    protected virtual void Update()
    {
        if (isGameRuning == false)
            return;

        if (_behaviourTree == null)
            return;

        CheckInputCard();
        CheckInCome();
        _behaviourTree.Do(this);

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        BaseMonster.OnGoal -= OnGoal;
        BaseMonster.OnDeath -= OnDeath;
    }

    #endregion

    #region :   Protected

    protected virtual void GameStart()
    {
        isGameRuning = true;

        //Default Card Setting
        SetDefaultCard();
    }

    protected virtual void InitializeGameData()
    {
        if (_stageManager.TryGetStageData(out var stageData) == false)
            return;

        if (_stageManager.TryGetStageMonsterData(out var stageMonsterData) == false)
            return;

        incomePerTime = stageData.Income_Second;

        _inGameDataManager.InCome = stageData.Income;
        _inGameDataManager.HP = stageData.HP;
        _inGameDataManager.currentStage = stageMonsterData.Stage;
        _inGameDataManager.currentWave = stageMonsterData.Wave;

        var monsterTotalQuantity = _stageManager.GetStageMonsterTotalQuantity(_inGameDataManager.currentStage);
        _inGameDataManager.currentMonsterTotalQuantity = monsterTotalQuantity;
        _inGameDataManager.currentMonsterKilledTotalQuantity = 0;
    }

    protected virtual void InitializeUI()
    {
        var InGame_PANEL_PATH = "Game/InGamePanel";

        //UI Callgback 및 관리
        inGamePanel = _uiManager.Show<PVEGamePanelController>(InGame_PANEL_PATH);
        inGamePanel.UpdateIncomeData();
        inGamePanel.OnSelectCard += SelectCard;


        cardSetController = _uiManager.Show<CardSetController>("Game/CardPanel");
        cardSetController.OnSetCard = SetCard;
        cardSetController.OnRotateCard = RotateCard;
        cardSetController.Hide();
    }

    protected virtual void InitializeMap()
    {
        var result = Resources.Load<GameObject>("Maps/Map_0");
        if (result == null)
            return;

        var mapObject = GameObject.Instantiate(result, Vector3.zero, Quaternion.identity);
        _mapController = mapObject.GetComponent<MapController>();
    }

    protected virtual void OnCreateMonster(StageMonsterData stageMonsterData)
    {
        var monsterStatusData = _characterManager.GetMonsterStatusData(stageMonsterData.Character_ID);
        if (monsterStatusData == null)
            return;

        var loadPrefab = _characterManager.GetMonster(stageMonsterData.Character_ID);
        if (loadPrefab == null)
            return;

        var monster = Instantiate(loadPrefab, Vector3.zero, Quaternion.identity);
        if (monster == null)
            return;

        monster.Initialize(_mapController, monsterStatusData);

        var wave = _inGameDataManager.currentWave;
        if (_waveMonsterDictionary.ContainsKey(wave) == false)
            _waveMonsterDictionary.Add(wave, new List<BaseMonster>());

        _waveMonsterDictionary[wave].Add(monster);

        _stageManager.AddMonster(monster);

        inGamePanel.AddViewMonsterCount();
    }

    protected virtual int GetCurrentMonsterQuantity()
    {
        var quantity = 0;
        var enumerator = _waveMonsterDictionary.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            var monsters = current.Value;
            if (monsters == null)
                continue;

            quantity += monsters.Count;
        }

        return quantity;
    }

    protected virtual bool IsAllMonsterGoal()
    {
        if (IsFullWaveMonster() == false)
            return false;

        var enumerator = _waveMonsterDictionary.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            var monsters = current.Value;
            if (monsters == null)
                return false;

            for (int i = 0, length = monsters.Count; i < length; ++i)
            {
                var monster = monsters[i];
                if (monster.IsGoal() == false)
                    return false;
            }
        }

        return true;
    }

    #endregion

    #region Card
    private void SetDefaultCard()
    {
        Card card = new Card();
        int key = _cardManager.StartCardID;
        card.Initialize(_cardManager.CardDictionary[key]);

        _mapController.SetTileCharacter(card);
    }

    private void SelectCard(Card card)
    {
        if (card != null)
        {
            cardSetController.Show();
            Debug.Log($"카드선택: {card.CardData.iD}");
        }
        else
        {
            cardSetController.Hide();
        }

        curSelectedCard = card;

        isPossibleSetCard = _mapController.HighLightCard(curSelectedCard);
        SetCardSettingUIPostion();
    }

    private void CheckInputCard()
    {
        if (curSelectedCard == null)
            return;

        //Todo: 터치 적용: 현재는 마우스만
        if(Input.GetMouseButtonDown(0))
        {
            Vector3Int selectTilePos = _mapController.GetInputPosition();
            var rect = curSelectedCard.GetTileRect();
            bool isInInputPos = rect.IsInTileAtPos(selectTilePos);

            isPickCard = isInInputPos;
        }
        else if(Input.GetMouseButton(0))
        {
            if(isPickCard == true)
            {
                Vector3Int selectTilePos = _mapController.GetInputPosition();
                curSelectedCard.CardPos = selectTilePos;
                //카드 max,min
                _mapController.SetCardBoundsPostion(curSelectedCard);
                SetCardSettingUIPostion();
                isPossibleSetCard = _mapController.HighLightCard(curSelectedCard);
            }
        }
        else
        {
            isPickCard = false;
        }
    }
    private void SetCardSettingUIPostion()
    {
        if (curSelectedCard == null)
            return;

        var grid = _mapController.Grid;
        var interval = new Vector3(grid.cellSize.x / 2, -grid.cellSize.y * 1.5f);
        var pos = grid.CellToWorld(curSelectedCard.CardPos) + interval;
        cardSetController.SetPostion(pos);
    }
    private void RotateCard()
    {
        curSelectedCard.Rotate();
        _mapController.SetCardBoundsPostion(curSelectedCard);

        _mapController.HighLightCard(curSelectedCard);
    }

    private void SetCard()
    {
        if(isPossibleSetCard)
        {
            _mapController.SetTileCharacter(curSelectedCard);
            cardSetController.Hide();

            inGamePanel.SetCardInMap();

            curSelectedCard = null;
        }
    }

    #endregion

    #region InGame
    private void CheckInCome()
    {
        if(curIncomeTime > incomeInterval)
        {
            curIncomeTime -= incomeInterval;
            _inGameDataManager.InCome += incomePerTime;
            inGamePanel.UpdateIncomeData();
        }

        curIncomeTime += Time.deltaTime;
    }
    #endregion

    private void OnDeath(BaseMonster obj)
    {
        ++_inGameDataManager.currentMonsterKilledTotalQuantity;

        _stageManager.RemoveMonster(obj);

        inGamePanel.RemoveViewMonsterCount();
    }

    private void OnGoal(BaseMonster obj)
    {
        var statusData = obj.GetStatusData();
        if (statusData == null)
            return;

        _stageManager.RemoveMonster(obj);

        _inGameDataManager.HP -= statusData.Damage;

        inGamePanel.SetHP(_inGameDataManager.HP);

        _cameraController.Shake(_shakeScale, _shakeTime);
    }
}
