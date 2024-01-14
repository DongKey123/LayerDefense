using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    //Todo: LazySingleTon으로 변경
    public Tile defaultTile;
    public Tile monsterTile;

    //BackGround Tile
    public Tile greenTile;
    public Tile redTile;

    //Character Tile
    public Tile characterTile;
}
