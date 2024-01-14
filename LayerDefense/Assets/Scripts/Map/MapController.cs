using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    [Header ("WayPoint")]
    [SerializeField] protected Tilemap[] _waypointTilemaps = null;
    private List<Vector3> _waypointList = new List<Vector3>();

    [Header("Setting")]
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap playerGroundTilemap = null;
    [SerializeField] private Tilemap highlightTilemap = null;
    [SerializeField] private Tilemap previewTilemap = null;
    public Grid Grid { get { return grid;} }

    private MapManager mapManager = null;
    private CharacterManager characterManager = null;
    
    //Data <= Todo ???? ????
    private Dictionary<Vector3Int, BaseHero> tileObjects = new Dictionary<Vector3Int, BaseHero>();

    #region :   Unity Message

    protected virtual void Awake()
    {
        mapManager = MapManager.getInstance;
        characterManager = CharacterManager.getInstance;


        InitializeWaypoint();

    }

#endregion

    #region Set Card
    public bool HighLightCard(Card card)
    {
        highlightTilemap.ClearAllTiles();
        previewTilemap.ClearAllTiles();

        if (card == null)
            return false;

        TileRect rect = card.GetTileRect();
        int left = rect.leftTopPos.x;
        int top = rect.leftTopPos.y;
        int right = rect.RightBottomPos.x;
        int bottom = rect.RightBottomPos.y;

        //TileCheck
        int count = 0;
        for (int i = left; i <= right; i++)
        {
            for (int j = bottom; j <= top; j++)
            {
                var tilePos = new Vector3Int(i, j, 0);

                if (playerGroundTilemap.GetTile(tilePos) == mapManager.defaultTile)
                    count++;
            }
        }
        
        //?????? ???? ?? ???? ??
        bool isSetCard = count > 1;

        for (int i = left; i <= right; i++)
        {
            for (int j = bottom; j <= top; j++)
            {
                var tilePos = new Vector3Int(i, j, 0);
                var tileLocalPos = new Vector3Int(i - left, j - bottom, 0);
                bool isUnitTile = card.GetIncludeTile(tileLocalPos.x, tileLocalPos.y);

                if (isSetCard == true)
                {
                    highlightTilemap.SetTile(tilePos, mapManager.greenTile);
                }
                else
                {
                    highlightTilemap.SetTile(tilePos, mapManager.redTile);
                }

                if (isUnitTile)
                {
                    //Todo 2D?? ???? ?? Tile?? ???? ?? ????...
                    Tile tile = characterManager.GetCharacterTile(card.GetCardData(tileLocalPos.x, tileLocalPos.y));
                    previewTilemap.SetTile(tilePos, tile);
                }
            }
        }

        return isSetCard;
    }

    public void SetTileCharacter(Card card)
    {
        highlightTilemap.ClearAllTiles();
        previewTilemap.ClearAllTiles();

        TileRect rect = card.GetTileRect();
        int left = rect.leftTopPos.x;
        int top = rect.leftTopPos.y;
        int right = rect.RightBottomPos.x;
        int bottom = rect.RightBottomPos.y;

        for (int i = left; i <= right; i++)
        {
            for (int j = bottom; j <= top; j++)
            {
                var tilePos = new Vector3Int(i, j, 0);
                var tileLocalPos = new Vector3Int(i - left, j - bottom,0);

                playerGroundTilemap.SetTile(tilePos, mapManager.defaultTile);
                bool isUnitTile = card.GetIncludeTile(tileLocalPos.x, tileLocalPos.y);


                if(isUnitTile)
                {
                    var data = card.GetCardData(tileLocalPos.x, tileLocalPos.y);
                    var hero = characterManager.GetHero(data.Character_ID);
                    var heroStatusData = characterManager.GetHeroStatusData(data.Character_ID);
                    //Todo: ObjectPool
                    var obj = Instantiate(hero);
                    obj.Initialize(heroStatusData);
                    obj.transform.position = grid.GetCellCenterWorld(tilePos);

                    SetCharacter(tilePos, obj);
                }
                else
                {
                    SetCharacter(tilePos, null);
                }
            }
        }
    }

    public Vector3Int GetInputPosition()
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

    public void SetCardBoundsPostion(Card card)
    {
        Vector3Int pos;

        int xPos = 0;
        int yPos = 0;

        var bound = playerGroundTilemap.cellBounds;

        switch (card.SetType)
        {
            case ECardSetType.Up:
                xPos = Mathf.Clamp(card.CardPos.x, bound.xMin + 1, bound.xMax - 2);
                yPos = Mathf.Clamp(card.CardPos.y, bound.yMin, bound.yMax - 2);
                break;
            case ECardSetType.Right:
                xPos = Mathf.Clamp(card.CardPos.x, bound.xMin, bound.xMax - 2);
                yPos = Mathf.Clamp(card.CardPos.y, bound.yMin + 1 , bound.yMax - 2);
                break;
            case ECardSetType.Bottom:
                xPos = Mathf.Clamp(card.CardPos.x, bound.xMin + 1, bound.xMax - 2);
                yPos = Mathf.Clamp(card.CardPos.y, bound.yMin + 1, bound.yMax - 1);
                break;
            case ECardSetType.Left:
                xPos = Mathf.Clamp(card.CardPos.x, bound.xMin+ 1, bound.xMax - 1);
                yPos = Mathf.Clamp(card.CardPos.y, bound.yMin + 1, bound.yMax - 2);
                break;
        }

        pos = new Vector3Int(xPos,yPos,0);
        card.CardPos = pos;
    }

    private void SetCharacter(Vector3Int tilePos, BaseHero obj)
    {
        if (tileObjects.ContainsKey(tilePos))
        {
            var tileHero = tileObjects[tilePos];

            if(obj != null && tileHero != null)
            {
                var tileID = tileHero.GetCharacterID();
                var objID = obj.GetCharacterID();

                if (tileID == objID)
                {
                    //MergeCharacter
                    Debug.Log("Merge: "+tileHero.GetCharacterID());
                }
                else
                {
                    Destroy(tileHero.gameObject);
                }
            }
            else
            {
                if(tileHero != null)
                    Destroy(tileHero.gameObject);
            }

            tileObjects[tilePos] = obj;
        }
        else // Tile Null
        {
            tileObjects.Add(tilePos, obj);
        }
    }
    #endregion

    #region WayPoint
    protected virtual void InitializeWaypoint()
    {
        if (_waypointTilemaps == null)
            return;

        for (int i = 0; i < _waypointTilemaps.Length; ++i)
        {
            foreach (var v in _waypointTilemaps[i].cellBounds.allPositionsWithin)
            {
                if (_waypointTilemaps[i].HasTile(v) == false)
                    continue;

                var waypoint = _waypointTilemaps[i].GetCellCenterWorld(v);

                _waypointList.Add(waypoint);
            }
        }
    }

    public Vector3 GetWaypoint(int waypointIndex)
    {
        if (waypointIndex < 0)
            return Vector3.zero;

        if (_waypointList == null)
            return Vector3.zero;

        if (_waypointList.Count <= waypointIndex)
            return Vector3.zero;

        return _waypointList[waypointIndex];
    }

    public bool IsExitWaypoint(int waypointIndex)
    {
        if (waypointIndex < 0)
            return true;

        if (_waypointList == null)
            return true;

        return _waypointList.Count - 1 <= waypointIndex;
    }

#endregion
}
