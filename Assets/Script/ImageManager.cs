using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : Singleton<ImageManager>
{
    public float ruleGap;
    [Serializable]
    public struct CellSpritePair
    {
        public Cell cell;
        public Sprite sprite;
    }
    public CellSpritePair[] sprites;
    public Dictionary<Cell, Sprite> cellSpriteDict;
    // Start is called before the first frame update
    void Awake()
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
