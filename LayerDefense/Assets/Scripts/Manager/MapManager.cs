using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : LazySingleton<MapManager>
{
    //Todo: 맵이 바뀜에 따라 데이터 받아오는 부분 테이블로 수정


    //BackGround Tile
    public Tile defaultTile;
    public Tile greenTile;
    public Tile redTile;

    MapManager()
    {
        LoadData();
    }

    public void LoadData()
    {
        defaultTile = Resources.Load<Tile>("Maps/Tilemap/desert_10");
        greenTile = Resources.Load<Tile>("Maps/Tilemap/desert_34");
        redTile = Resources.Load<Tile>("Maps/Tilemap/desert_46");
    }
}
