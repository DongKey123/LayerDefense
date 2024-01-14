using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECellType
{
    A1, A2, A3, A4, A5, 
    B1, B2, B3, B4, B5,
    C1, C2, C3, C4, C5,
    D1, D2, D3, D4, D5,
};

[Serializable]
public class CardData 
{
    public int iD;
    public ECellType cellType;
    public int cell1;
    public int cell2;
    public int cell3;
    public int cell4;
    public int cell5;
    public int cell6;
    public int cardCost;
}
