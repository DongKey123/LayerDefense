using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : LazySingleton<MapManager>
{
    //Todo: ���� �ٲ� ���� ������ �޾ƿ��� �κ� ���̺�� ����


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
