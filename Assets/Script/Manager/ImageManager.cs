using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : Singleton<ImageManager>
{
    public float ruleGap;
    public GameObject cellPrefab, cellPrefabInRule, cellPrefabInRuleIO, cellPrefabInPalette;
    public GameObject mapBackgroundPrefab, rulePrefab, palettePrefab, symbolPrefab;
    public GameObject ruleButtonPrefab, constraintButtonPrefab, constraintNumButtonPrefab;
    public GameObject mapResizerPrefab;
    [Serializable]
    public struct CellSpritePair
    {
        public Cell cell;
        public Sprite sprite;
    }
    public CellSpritePair[] cellSprites;
    public Dictionary<Cell, Sprite> cellSpriteDict;
    [Serializable]
    public struct SymbolSpritePair
    {
        public ConstraintType symbol;
        public Sprite sprite;
    }
    public SymbolSpritePair[] symbolSprites;
    public Dictionary<ConstraintType, Sprite> symbolSpriteDict;
    public Sprite[] numberSprites;
    public Sprite[] mapBackgroundSprites;
    public Sprite[] cellNumSprites;
    public Sprite[] ruleNumSprites;
    public Sprite[] buttonSprites;
    public Sprite[] backgroundSprites;
    public Sprite[] topBoardSprites;
    public Sprite[] ruleEditButtonSprites;
    public Sprite[] ruleResetButtonSprites;
    public Sprite[] constraintNumButtonSprites;
    public Sprite ruleCellSprite;
    public Sprite[] mapResizerSprites;
    public Sprite[] editorButtonSprites;
    public Sprite editorButtonSelectedSprite;
    // Start is called before the first frame update
    void Start()
    {
        cellSpriteDict = new Dictionary<Cell, Sprite>();
        symbolSpriteDict = new Dictionary<ConstraintType, Sprite>();

        foreach (CellSpritePair cs in cellSprites)
        {
            cellSpriteDict.Add(cs.cell, cs.sprite);
        }
        foreach (SymbolSpritePair ss in symbolSprites)
        {
            symbolSpriteDict.Add(ss.symbol, ss.sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
