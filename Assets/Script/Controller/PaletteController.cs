using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteController : MonoBehaviour
{
    private PaletteCellController[] cells;
    private int[] cellsNum;

    public void PaletteInstantiate(List<CellNumPair> palette)
    {
        cells = new PaletteCellController[palette.Count];
        cellsNum = new int[palette.Count];

        for (int i = 0; i < palette.Count; ++i)
        {
            cells[i] = Instantiate(ImageManager.Inst.cellPrefabInPalette, transform).GetComponent<PaletteCellController>();
            cells[i].transform.localPosition = new Vector2(i, 0);
            cells[i].CellInitialize(palette[i].cell, i);
            cells[i].CellNumInitialize(cellsNum[i] = palette[i].num);
        }
    }
}
