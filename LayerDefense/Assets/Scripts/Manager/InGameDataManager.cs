using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameDataManager : LazySingleton<InGameDataManager>
{
    //InGame ?? Model
    public readonly int REROLL_COST = 5;
    public readonly int ADD_RECOME = 10;

    public int InCome { get { return inCome; } set { inCome = value; } }
    public int HP { get { return hp; } set { hp = value; } }

    public int currentStage = 0;
    public int currentWave = 0;
    public int currentMonsterTotalQuantity = 0;
    public int currentMonsterKilledTotalQuantity = 0;

    private int inCome = 0;
    private int hp = 0;

    public void ResetData()
    {
        inCome = 0;
        hp = 0;

        currentStage = 0;
        currentWave = 0;
        currentMonsterTotalQuantity = 0;
        currentMonsterKilledTotalQuantity = 0;
    }
}