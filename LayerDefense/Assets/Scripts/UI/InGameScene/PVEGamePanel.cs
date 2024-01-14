using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PVEGamePanel : MonoBehaviour
{
    
    #region Field

    [SerializeField] private TextMeshProUGUI curMonsterCountText;
    [SerializeField] private TextMeshProUGUI curIncomeMoneyText;
    [SerializeField] private Slider hpBar;

    [SerializeField] private Button rorollButton;
    [SerializeField] private TextMeshProUGUI rerollPayText;
    #endregion

    private const string MonsterCountString = "Monster Count";
    private const string IncomeMoneyString = "IncomeMoney";

    #region TopUI

    private int monsterCount = 0;

    public void SetMonsterCount(int count)
    {
        curMonsterCountText.text = $"{MonsterCountString}: {count}";
    }

    public void AddMonsterCount()
    {
        ++monsterCount;
        curMonsterCountText.text = $"{MonsterCountString}: {monsterCount}";
    }

    public void RemoveMonsterCount()
    {
        --monsterCount;
        curMonsterCountText.text = $"{MonsterCountString}: {monsterCount}";
    }

    public void SetMaxHp(int maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;
    }

    public void SetHp(int curHp)
    {
        hpBar.value = curHp;
    }
    #endregion

    #region InGameUI

    public void SetIncomeMoney(int income)
    {
        curIncomeMoneyText.text = $"{IncomeMoneyString}: {income}";
    }

    public void SetReRollMoney(int rerollPay)
    {
        rerollPayText.text = $"{rerollPay}";
    }

    public void SetEnableIncomeButton(bool enable)
    {
        rorollButton.interactable = enable;
    }
    #endregion
}
