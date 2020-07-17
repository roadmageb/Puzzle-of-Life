using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public void ChangeSprite(Cell cell)
    {
        GetComponent<SpriteRenderer>().sprite = CellManager.Inst.cellSpriteDict[cell];
    }
}
