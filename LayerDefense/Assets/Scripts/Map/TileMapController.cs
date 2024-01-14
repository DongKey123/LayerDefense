using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public enum Edge
{
    LeftTop,
    RightTop,
    RightBottom,
    LeftBottom,
}

//Todo: LazySingle? MonoSingle? 싱글톤으로 교체
public class TileMapController : MonoBehaviour
{
    [SerializeField] private MapManager tileMapManager = null;
    [SerializeField] private Grid grid = null;
    [SerializeField] private Tilemap backgroundTileMap = null;
    [SerializeField] private Tilemap HighLightTileMap = null;
    [SerializeField] private Tilemap HighLightCharacterMap = null;

    [SerializeField] private int mapWidth = 5;
    [SerializeField] private int mapHeight = 5;

    [Header("���� UI")]
    [SerializeField] private Canvas worldUICanvas = null;
    private RectTransform worldUICanvasRectTransform = null;
    [SerializeField] private GameObject selectUI = null;
    private RectTransform selectRectTransform = null;

    [Header("Todo Change")]
    [SerializeField] GameObject hero = null;

    private bool isSelctCard = false;
    private bool isPickCard = false;
    private Vector3Int curSelectPos = Vector3Int.zero;
    private Vector3Int curSelectLeftTopPos = Vector3Int.zero;
    private Vector3Int curSelectRightBottomPos = Vector3Int.zero;

    //Data
    private Dictionary<Vector3Int, GameObject> tileObjects = new Dictionary<Vector3Int, GameObject>();


    //Todo: 상호참조 관련 수정
    private CardController cardController = null;

    private CardElementController curSelectedCard = null;

    //Caching
    private CharacterManager characterManager;

    //Event
    public Action<int> OnSetHero;

    private void Awake()
    {
        selectRectTransform = selectUI.GetComponent<RectTransform>();
        worldUICanvasRectTransform = worldUICanvas.GetComponent<RectTransform>();

        cardController = FindObjectOfType<CardController>();
        characterManager = CharacterManager.getInstance;

        Debug.Log(backgroundTileMap.cellBounds);
    }

    /// <summary>
    /// 카드 설치 관련 Update 처
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSelctCard)
        {
            Vector3Int selectTilePos = GetTouchPosition();
            if (selectTilePos.x >= curSelectLeftTopPos.x && selectTilePos.x <= curSelectRightBottomPos.x &&
                selectTilePos.y >= curSelectRightBottomPos.y && selectTilePos.y <= curSelectLeftTopPos.y)
            {
                isPickCard = true;
            }

            if (isPickCard)
            {
                curSelectPos = selectTilePos;
                SetTileRect();
                SetCardHighLight(selectTilePos.x, selectTilePos.y, curSelectedCard.CurSelectSetType);
                SetHighLightUIPos(selectTilePos.x, selectTilePos.y);
            }
        }
        else if (Input.GetMouseButton(0) && isSelctCard)
        {
            Vector3Int selectTilePos = GetTouchPosition();

            if (isPickCard)
            {
                curSelectPos = selectTilePos;
                SetTileRect();

                bool isShow = true;

                if (curSelectLeftTopPos.x < -mapWidth / 2)
                {
                    isShow = false;
                    selectTilePos.x = -mapWidth / 2;
                }
                else if (curSelectRightBottomPos.x > mapWidth / 2 - 1)
                {
                    isShow = false;
                    selectTilePos.x = mapWidth / 2 - 1;
                }

                if (curSelectRightBottomPos.y < -mapHeight / 2)
                {
                    isShow = false;
                    selectTilePos.y = -mapWidth / 2;
                }
                else if (curSelectLeftTopPos.y > mapHeight / 2 - 1)
                {
                    isShow = false;
                    selectTilePos.y = -mapWidth / 2 - 1;
                }


                if (isShow)
                {
                    SetCardHighLight(selectTilePos.x, selectTilePos.y, curSelectedCard.CurSelectSetType);
                    SetHighLightUIPos(selectTilePos.x, selectTilePos.y);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPickCard = false;
        }

    }

    /// <summary>
    /// 선택 된 맵 하이라이팅 초록색 , 붉은색
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetCardHighLightTile(int x, int y)
    {
        isSelctCard = true;

        curSelectPos = new Vector3Int(x, y, 0);
        SetTileRect();

        var curSelectSetType = curSelectedCard == null ? ECardSetType.Up : curSelectedCard.CurSelectSetType;

        SetCardHighLight(x, y, curSelectSetType);

        SetHighLightUIPos(x, y);
    }

    public void DeSelectHighLightTile()
    {
        isSelctCard = false;

        HighLightTileMap.ClearAllTiles();
        HighLightCharacterMap.ClearAllTiles();

        selectUI.SetActive(false);
    }

    public void SetTileHeroDefault()
    {
        HighLightTileMap.ClearAllTiles();
        isSelctCard = false;

        SetTileRect();


        int left = curSelectLeftTopPos.x;
        int top = curSelectLeftTopPos.y;
        int right = curSelectRightBottomPos.x;
        int bottom = curSelectRightBottomPos.y;

        for (int i = left; i <= right; i++)
        {
            for (int j = bottom; j <= top; j++)
            {
                var tilePos = new Vector3Int(i, j, 0);

                var obj = Instantiate(hero);
                obj.transform.position = grid.GetCellCenterWorld(tilePos);
                backgroundTileMap.SetTile(tilePos, tileMapManager.defaultTile);

                SetCharacter(tilePos, obj);
            }
        }
    }

    public void SetTileHero()
    {
        HighLightTileMap.ClearAllTiles();
        HighLightCharacterMap.ClearAllTiles();


        if (curSelectedCard == null)
            Debug.LogError("선택한 카드가 없습니다.");

        int left = curSelectLeftTopPos.x;
        int top = curSelectLeftTopPos.y;
        int right = curSelectRightBottomPos.x;
        int bottom = curSelectRightBottomPos.y;

        var card = curSelectedCard;

        for (int i = left; i <= right; i++)
        {
            for (int j = bottom; j <= top; j++)
            {
                var tilePos = new Vector3Int(i, j, 0);

                bool isUnitTile = curSelectedCard.GetIncludedTile(i - left, j - bottom);

                backgroundTileMap.SetTile(tilePos, tileMapManager.defaultTile);

                if (isUnitTile)
                {
                    var hero = characterManager.GetHero(curSelectedCard.GetCardData(i - left, j - bottom).Character_ID);
                    if (hero != null)
                    {
                        var obj = Instantiate(hero);
                        obj.transform.position = grid.GetCellCenterWorld(tilePos);

                        SetCharacter(tilePos, obj.gameObject);
                    }
                }
                else
                {
                    SetCharacter(tilePos, null);
                }
            }
        }
        OnSetHero?.Invoke(card.cardCost);


        cardController.SetCard();
    }

    private void SetHighLightUIPos(int x, int y)
    {
        var localPos = new Vector3Int(x, y - 2, 0);
        var worldPos = HighLightTileMap.CellToWorld(localPos);
        worldPos += HighLightTileMap.cellSize / 2;
        selectUI.SetActive(true);


        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(worldPos);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * worldUICanvasRectTransform.sizeDelta.x) - (worldUICanvasRectTransform.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * worldUICanvasRectTransform.sizeDelta.y) - (worldUICanvasRectTransform.sizeDelta.y * 0.5f)));

        selectRectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }

    private Vector3Int GetTouchPosition()
    {
        Vector3 worldPos = Vector3.zero;

#if UNITY_STANDALONE || UNITY_EDITOR
        worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

#elif UNITY_IOS || UNITY_ANDROID
        if(Input.touchCount > 0)
        {
            worldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
#endif
        return grid.WorldToCell(worldPos);
    }

    private void SetTileRect()
    {
        ECardSetType cardSetType;

        if (curSelectedCard == null)
            cardSetType = ECardSetType.Up;
        else
            cardSetType = curSelectedCard.CurSelectSetType;


        switch (cardSetType)
        {
            case ECardSetType.Up:
                curSelectLeftTopPos = curSelectPos + new Vector3Int(-1, +1, 0);
                curSelectRightBottomPos = curSelectPos + new Vector3Int(+1, 0, 0);
                break;
            case ECardSetType.Right:
                curSelectLeftTopPos = curSelectPos + new Vector3Int(0, +1, 0);
                curSelectRightBottomPos = curSelectPos + new Vector3Int(+1, -1, 0);
                break;
            case ECardSetType.Bottom:
                curSelectLeftTopPos = curSelectPos + new Vector3Int(-1, 0, 0);
                curSelectRightBottomPos = curSelectPos + new Vector3Int(+1, -1, 0);
                break;
            case ECardSetType.Left:
                curSelectLeftTopPos = curSelectPos + new Vector3Int(-1, +1, 0);
                curSelectRightBottomPos = curSelectPos + new Vector3Int(0, -1, 0);
                break;
        }
    }

    private void SetCharacter(Vector3Int tilePos, GameObject obj)
    {
        if (tileObjects.ContainsKey(tilePos))
        {
            var tileObj = tileObjects[tilePos];
            GameObject.Destroy(tileObj);
            tileObjects[tilePos] = obj;
        }
        else
        {
            tileObjects.Add(tilePos, obj);
        }
    }

    private void SetCardHighLight(int x, int y, ECardSetType cardSetType)
    {
        HighLightTileMap.ClearAllTiles();
        HighLightCharacterMap.ClearAllTiles();

        curSelectedCard = cardController.CurSelectedCard;

        if (curSelectedCard == null)
            return;


        int left = curSelectLeftTopPos.x;
        int top = curSelectLeftTopPos.y;
        int right = curSelectRightBottomPos.x;
        int bottom = curSelectRightBottomPos.y;

        //TileCheck
        int count = 0;
        for (int i = left; i <= right; i++)
        {
            for (int j = bottom; j <= top; j++)
            {
                var tilePos = new Vector3Int(i, j, 0);

                if (backgroundTileMap.GetTile(tilePos) == tileMapManager.defaultTile)
                    count++;

            }
        }

        for (int i = left; i <= right; i++)
        {
            for (int j = bottom; j <= top; j++)
            {
                var tilePos = new Vector3Int(i, j, 0);
                bool isUnitTile = curSelectedCard.GetIncludedTile(i - left, j - bottom);
                //Tile tile 


                if (count > 1)
                {
                    HighLightTileMap.SetTile(tilePos, tileMapManager.greenTile);
                }
                else
                {
                    HighLightTileMap.SetTile(tilePos, tileMapManager.redTile);
                }

                if (isUnitTile)
                {
                    Tile tile = characterManager.GetCharacterTile(curSelectedCard.GetCardData(i - left, j - bottom));
                    HighLightCharacterMap.SetTile(tilePos, tile);
                }
            }
        }
    }

    public Vector3 GetEdgePosition(Edge edge)
    {
        Vector3 pos;

        switch (edge)
        {
            case Edge.LeftTop:
                Vector3Int leftTop = new Vector3Int(-mapWidth / 2, mapHeight / 2 - 1, 0);
                pos = grid.GetCellCenterWorld(leftTop);
                break;
            case Edge.RightTop:
                Vector3Int rightTop = new Vector3Int(mapWidth / 2 - 1, mapHeight / 2 - 1, 0);
                pos = grid.GetCellCenterWorld(rightTop);
                break;
            case Edge.RightBottom:
                Vector3Int rightBottom = new Vector3Int(mapWidth / 2 - 1, -mapHeight / 2, 0);
                pos = grid.GetCellCenterWorld(rightBottom);
                break;
            case Edge.LeftBottom:
                Vector3Int leftBottom = new Vector3Int(-mapWidth / 2, -mapHeight / 2, 0);
                pos = grid.GetCellCenterWorld(leftBottom);
                break;

            default:
                pos = Vector3.zero;
                break;
        }


        return pos;
    }
}