using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public void ChangeSprite(Cell cell)
    {
        Transform cellForeground = transform.GetChild(0);
        cellForeground.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellSpriteDict[cell];
    }
}
