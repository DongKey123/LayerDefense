using UnityEngine;

public struct TileRect
{
    public Vector3Int leftTopPos;
    public Vector3Int RightBottomPos;

    public bool IsInTileAtPos(Vector3Int pos)
    {
        if (pos.x < leftTopPos.x)
            return false;

        if (pos.x > RightBottomPos.x)
            return false;

        if (pos.y > leftTopPos.y)
            return false;

        if (pos.y < RightBottomPos.y)
            return false;

        return true;
    }
}

public class Card : BaseCard
{
    private CharacterData[] characterDatas = new CharacterData[6];
    
    public Vector3Int CardPos { set { cardPos = value; } get { return cardPos; } }
    private Vector3Int cardPos = Vector3Int.zero;
    

    public override void Initialize(CardData data)
    {
        var characterManager = CharacterManager.getInstance;
        cardData = data;

        characterDatas[0] = cardData.cell1 != 0 ? characterManager.GetRandomHero() : null;
        characterDatas[1] = cardData.cell2 != 0 ? characterManager.GetRandomHero() : null;
        characterDatas[2] = cardData.cell3 != 0 ? characterManager.GetRandomHero() : null;
        characterDatas[3] = cardData.cell4 != 0 ? characterManager.GetRandomHero() : null;
        characterDatas[4] = cardData.cell5 != 0 ? characterManager.GetRandomHero() : null;
        characterDatas[5] = cardData.cell6 != 0 ? characterManager.GetRandomHero() : null;
    }

    public override void Rotate()
    {
        switch (setType)
        {
            case ECardSetType.Up:
                setType = ECardSetType.Right;
                break;
            case ECardSetType.Right:
                setType = ECardSetType.Bottom;
                break;
            case ECardSetType.Bottom:
                setType = ECardSetType.Left;
                break;
            case ECardSetType.Left:
                setType = ECardSetType.Up;
                break;
        }
    }

    public TileRect GetTileRect()
    {
        TileRect rect = new TileRect();

        switch (setType)
        {
            case ECardSetType.Up:
                rect.leftTopPos = cardPos + new Vector3Int(-1, +1, 0);
                rect.RightBottomPos = cardPos + new Vector3Int(+1, 0, 0);
                break;
            case ECardSetType.Right:
                rect.leftTopPos = cardPos + new Vector3Int(0, +1, 0);
                rect.RightBottomPos = cardPos + new Vector3Int(+1, -1, 0);
                break;
            case ECardSetType.Bottom:
                rect.leftTopPos = cardPos + new Vector3Int(-1, 0, 0);
                rect.RightBottomPos = cardPos + new Vector3Int(+1, -1, 0);
                break;
            case ECardSetType.Left:
                rect.leftTopPos = cardPos + new Vector3Int(-1, +1, 0);
                rect.RightBottomPos = cardPos + new Vector3Int(0, -1, 0);
                break;
        }

        return rect;
    }

    public bool GetIncludeTile(int x, int y)
    {
        //Todo : 계산식 정규화 시키기 
        switch (setType)
        {
            case ECardSetType.Up:
                if (x == 0 && y == 0)
                {
                    return cardData.cell4 > 0;
                }
                else if (x == 1 && y == 0)
                {
                    return cardData.cell5 > 0;
                }
                else if (x == 2 && y == 0)
                {
                    return cardData.cell6 > 0;
                }
                else if (x == 0 && y == 1)
                {
                    return cardData.cell1 > 0;
                }
                else if (x == 1 && y == 1)
                {
                    return cardData.cell2 > 0;
                }
                else if (x == 2 && y == 1)
                {
                    return cardData.cell3 > 0;
                }
                break;
            case ECardSetType.Right:
                if (x == 0 && y == 0)
                {
                    return cardData.cell6 > 0;
                }
                else if (x == 0 && y == 1)
                {
                    return cardData.cell5 > 0;
                }
                else if (x == 0 && y == 2)
                {
                    return cardData.cell4 > 0;
                }
                else if (x == 1 && y == 0)
                {
                    return cardData.cell3 > 0;
                }
                else if (x == 1 && y == 1)
                {
                    return cardData.cell2 > 0;
                }
                else if (x == 1 && y == 2)
                {
                    return cardData.cell1 > 0;
                }
                break;
            case ECardSetType.Bottom:
                if (x == 0 && y == 0)
                {
                    return cardData.cell3 > 0;
                }
                else if (x == 1 && y == 0)
                {
                    return cardData.cell2 > 0;
                }
                else if (x == 2 && y == 0)
                {
                    return cardData.cell1 > 0;
                }
                else if (x == 0 && y == 1)
                {
                    return cardData.cell6 > 0;
                }
                else if (x == 1 && y == 1)
                {
                    return cardData.cell5 > 0;
                }
                else if (x == 2 && y == 1)
                {
                    return cardData.cell4 > 0;
                }
                break;
            case ECardSetType.Left:
                if (x == 0 && y == 0)
                {
                    return cardData.cell1 > 0;
                }
                else if (x == 1 && y == 0)
                {
                    return cardData.cell4 > 0;
                }
                else if (x == 0 && y == 1)
                {
                    return cardData.cell2 > 0;
                }
                else if (x == 1 && y == 1)
                {
                    return cardData.cell5 > 0;
                }
                else if (x == 0 && y == 2)
                {
                    return cardData.cell3 > 0;
                }
                else if (x == 1 && y == 2)
                {
                    return cardData.cell6 > 0;
                }
                break;
        }

        return false;

    }

    public CharacterData GetCardData(int x, int y)
    {
        switch (setType)
        {
            case ECardSetType.Up:
                //Todo: 수정해야함 테이블을 수정해야할라...?
                if (x == 0 && y == 0)
                {
                    return characterDatas[3];
                }
                else if (x == 1 && y == 0)
                {
                    return characterDatas[4];
                }
                else if (x == 2 && y == 0)
                {
                    return characterDatas[5];
                }
                else if (x == 0 && y == 1)
                {
                    return characterDatas[0];
                }
                else if (x == 1 && y == 1)
                {
                    return characterDatas[1];
                }
                else if (x == 2 && y == 1)
                {
                    return characterDatas[2];
                }
                break;
            case ECardSetType.Right:
                if (x == 0 && y == 0)
                {
                    return characterDatas[5];
                }
                else if (x == 0 && y == 1)
                {
                    return characterDatas[4];
                }
                else if (x == 0 && y == 2)
                {
                    return characterDatas[3];
                }
                else if (x == 1 && y == 0)
                {
                    return characterDatas[2];
                }
                else if (x == 1 && y == 1)
                {
                    return characterDatas[1];
                }
                else if (x == 1 && y == 2)
                {
                    return characterDatas[0];
                }
                break;
            case ECardSetType.Bottom:
                if (x == 0 && y == 0)
                {
                    return characterDatas[2];
                }
                else if (x == 1 && y == 0)
                {
                    return characterDatas[1];
                }
                else if (x == 2 && y == 0)
                {
                    return characterDatas[0];
                }
                else if (x == 0 && y == 1)
                {
                    return characterDatas[5];
                }
                else if (x == 1 && y == 1)
                {
                    return characterDatas[4];
                }
                else if (x == 2 && y == 1)
                {
                    return characterDatas[3];
                }
                break;
            case ECardSetType.Left:
                if (x == 0 && y == 0)
                {
                    return characterDatas[0];
                }
                else if (x == 1 && y == 0)
                {
                    return characterDatas[3];
                }
                else if (x == 0 && y == 1)
                {
                    return characterDatas[1];
                }
                else if (x == 1 && y == 1)
                {
                    return characterDatas[4];
                }
                else if (x == 0 && y == 2)
                {
                    return characterDatas[2];
                }
                else if (x == 1 && y == 2)
                {
                    return characterDatas[5];
                }
                break;
        }

        return null;
    }

    public CharacterData[] GetCharacterDatas()
    {
        return characterDatas;
    }
}
