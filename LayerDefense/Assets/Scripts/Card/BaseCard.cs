using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECardSetType
{
    Up,
    Right,
    Bottom,
    Left
}

public abstract class BaseCard
{
    protected ECardSetType setType = ECardSetType.Up;
    protected CardData cardData = null;
    protected int cardCost = 0;


    public ECardSetType SetType { get { return setType; } }
    public CardData CardData { get { return cardData; } }
    public int CardCost { get { return cardCost; } set { cardCost = value; } }
    

    public abstract void Initialize(CardData data);
    public abstract void Rotate();
}
