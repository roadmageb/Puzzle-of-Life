using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteController : MonoBehaviour
{
    public CellController[] cells;
    public int[] cellsNum { get; set; }
    public void PaletteInstantiate(List<CellNumPair> palette)
    {
        cells = new CellController[palette.Count];
        cellsNum = new int[palette.Count];

        for (int i = 0; i < palette.Count; ++i)
        {
            cells[i] = Instantiate(ImageManager.Inst.cellPrefabInPalette, transform).GetComponent<CellController>();
            cells[i].transform.localPosition = new Vector2(i, 0);
            cells[i].CellInitialize(palette[i].cell, "Palette");
            cells[i].CellNumInitialize(cellsNum[i] = palette[i].num);
            cells[i].havingCellNum = palette[i].num;
        }
    }
}
