using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteController : MonoBehaviour
{
    public CellController[] cells;
    public GameObject[] cellsNum;
    public GameObject cellNumPrefab;
    public Vector2 textOffset;
    public void PaletteInstantiate(List<CellNumPair> palette)
    {
        cells = new CellController[palette.Count];
        cellsNum = new GameObject[palette.Count];

        for (int i = 0; i < palette.Count; ++i)
        {
            cells[i] = Instantiate(ImageManager.Inst.cellPrefab, transform).GetComponent<CellController>();
            cells[i].transform.localPosition = new Vector2(i, 0);
            cells[i].CellInitialize(palette[i].cell, true, "Palette");
            cellsNum[i] = Instantiate(cellNumPrefab, cells[i].transform);
            cellsNum[i].transform.localPosition = textOffset;
            cellsNum[i].GetComponent<TextMesh>().text = palette[i].num.ToString();
        }
    }
}
