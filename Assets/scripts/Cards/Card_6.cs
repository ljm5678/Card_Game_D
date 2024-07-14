using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_6 : CardItem
{
    public override void RefreshData(float zPos)
    {
        base.zPos = zPos;
        base.spriteRenderer.sortingOrder = (int)zPos + 1;
    }
}
