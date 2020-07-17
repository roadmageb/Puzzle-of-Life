using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : Singleton<CellManager>
{
    [Serializable]
    public struct CellSpritePair
    {
        public Cell cell;
        public Sprite sprite;
    }
    public CellSpritePair[] sprites;
    public Dictionary<Cell, Sprite> cellSpriteDict;
    // Start is called before the first frame update
    void Start()
    {
        cellSpriteDict = new Dictionary<Cell, Sprite>();

        foreach (CellSpritePair cs in sprites)
        {
            cellSpriteDict.Add(cs.cell, cs.sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
