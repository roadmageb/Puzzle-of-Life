using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private PlayState playState;
    public CellController cellUnderCursor { get; set; }
    public Transform mapOrigin;
    public Transform ruleOrigin;
    public Transform paletteOrigin;
    public SpriteRenderer background;
    public InGameButtonController buttonController;
    private Transform[,] mapBackgroundObject;
    public MapCellController[,] mapCellObject { get; private set; }
    public RuleController[] ruleObject { get; private set; }
    public PaletteController paletteObject { get; private set; }
    public Level currentLevel { get; set; }
    private Cell[,] previousCells;
    public float wholeRuleHeight { get; private set; }
    private float interval;
    private string currentLevelName;

    public GameObject ClearWindow;

    public void SetPlayState(PlayState playState)
    {
        this.playState = playState;
        buttonController.SetButtonForPlayState(playState);
        switch (playState)
        {
            case PlayState.PLAY:
            case PlayState.PLAYFRAME:
                background.sprite = ImageManager.Inst.backgroundSprites[1];
                break;
            case PlayState.EDIT:
            case PlayState.EDITTOINIT:
                background.sprite = ImageManager.Inst.backgroundSprites[0];
                break;
        }
    }

    public PlayState GetPlayState()
    {
        return playState;
    }

    public void PlayLevel()
    {
        if (playState == PlayState.PLAY) return;
        if (playState == PlayState.EDIT)
        {
            previousCells = new Cell[currentLevel.size.x, currentLevel.size.y];
            for (int i = 0; i < currentLevel.size.x; ++i)
            {
                for (int j = 0; j < currentLevel.size.y; ++j)
                {
                    previousCells[i, j] = currentLevel.map[i, j];
                }
            }
        }

        interval = 1.0f;
        StartCoroutine("CellCoroutine");
    }

    public void FastForwardLevel()
    {
        interval = Math.Max(interval / 1.5f, 0.2f);
    }

    public void PauseLevel()
    {
        StopCoroutine("CellCoroutine");
    }

    public void StopLevel()
    {
        background.sprite = ImageManager.Inst.backgroundSprites[0];
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                currentLevel.SetCell(new Vector2Int(i, j), previousCells[i, j]);
            }
        }

        CellUpdate();
        StopCoroutine("CellCoroutine");
    }

    public void PlayFrame()
    {
        if (playState == PlayState.EDIT)
        {
            previousCells = new Cell[currentLevel.size.x, currentLevel.size.y];
            for (int i = 0; i < currentLevel.size.x; ++i)
            {
                for (int j = 0; j < currentLevel.size.y; ++j)
                {
                    previousCells[i, j] = currentLevel.map[i, j];
                }
            }
        }

        currentLevel.NextState();
        CellUpdate();
    }

    private IEnumerator CellCoroutine()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                currentLevel.NextState();
                CellUpdate();
            }
            yield return null;
        }
    }

    private void CellInstantiate()
    {
        foreach (Transform child in mapOrigin)
        {
            Destroy(child.gameObject);
        }
        Vector2 offset = new Vector2((currentLevel.size.x + 1) / 2.0f, -(currentLevel.size.y + 1) / 2.0f);

        mapCellObject = new MapCellController[currentLevel.size.x, currentLevel.size.y];
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                mapCellObject[i, j] = Instantiate(ImageManager.Inst.cellPrefab, mapOrigin).GetComponent<MapCellController>();
                mapCellObject[i, j].transform.localPosition = new Vector2(i + 1, -(j + 1)) - offset;
                mapCellObject[i, j].CellInitialize(currentLevel.map[i, j], currentLevel.isReplaceable[i, j], new Vector2Int(i, j));
            }
        }

        mapBackgroundObject = new Transform[currentLevel.size.x + 2, currentLevel.size.y + 2];
        for (int i = 0; i < currentLevel.size.x + 2; ++i)
        {
            for (int j = 0; j < currentLevel.size.y + 2; ++j)
            {
                mapBackgroundObject[i, j] = Instantiate(ImageManager.Inst.mapBackgroundPrefab, mapOrigin).GetComponent<Transform>();
                mapBackgroundObject[i, j].localPosition = new Vector2(i, -j) - offset;
            }
        }
        for (int i = 1; i < currentLevel.size.x + 1; ++i)
        {
            for (int j = 1; j < currentLevel.size.y + 1; ++j)
            {
                mapBackgroundObject[i, j].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[4];
            }
        }
        for (int i = 1; i < currentLevel.size.x + 1; ++i)
        {
            mapBackgroundObject[i, 0].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[1]; // Top
            mapBackgroundObject[i, currentLevel.size.y + 1].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[7]; // Bottom
        }
        for (int j = 1; j < currentLevel.size.y + 1; ++j)
        {
            mapBackgroundObject[0, j].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[3]; // Left
            mapBackgroundObject[currentLevel.size.x + 1, j].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[5]; // Right
        }
        mapBackgroundObject[0, 0].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[0]; // Top-Left
        mapBackgroundObject[currentLevel.size.x + 1, 0].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[2]; // Top-Right
        mapBackgroundObject[0, currentLevel.size.y + 1].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[6]; // Bottom-Left
        mapBackgroundObject[currentLevel.size.x + 1, currentLevel.size.y + 1].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.mapBackgroundSprites[8]; // Bottom-Right
    }

    private void CellUpdate()
    {
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                mapCellObject[i, j].ChangeCell(currentLevel.map[i, j]);
            }
        }

        if (currentLevel.ClearCheck())
        {
            Debug.Log("clear");
            GameManager.Inst.ClearPuzzle(GameManager.Inst.stage, GameManager.Inst.level);
            ClearWindow.SetActive(true);
        }
    }

    private void RuleInstantiate()
    {
        foreach (Transform child in ruleOrigin)
        {
            Destroy(child.gameObject);
        }

        wholeRuleHeight = 0;
        ruleObject = new RuleController[currentLevel.rules.Count];

        for (int i = 0; i < currentLevel.rules.Count; ++i)
        {
            ruleObject[i] = Instantiate(ImageManager.Inst.rulePrefab, ruleOrigin).GetComponent<RuleController>();
            ruleObject[i].RuleInstantiate(currentLevel.rules[i], i);
            ruleObject[i].transform.localPosition = new Vector2(0, -wholeRuleHeight);
            wholeRuleHeight += ruleObject[i].ruleHeight + ImageManager.Inst.ruleGap;
        }
        wholeRuleHeight -= ImageManager.Inst.ruleGap;
    }

    private void PaletteInstantiate()
    {
        foreach (Transform child in paletteOrigin)
        {
            Destroy(child.gameObject);
        }

        paletteObject = Instantiate(ImageManager.Inst.palettePrefab, paletteOrigin).GetComponent<PaletteController>();
        paletteObject.PaletteInstantiate(currentLevel.palette);
    }
    private void MapScale(int criteria)
    {
        int maxValue = Math.Max(currentLevel.size.x, currentLevel.size.y);
        if (maxValue > criteria)
        {
            mapOrigin.localScale = new Vector3((float)criteria / maxValue, (float)criteria / maxValue, 1);
        }
        else
        {
            mapOrigin.localScale = new Vector3(1, 1, 1);
        }
    }

    public void MapReset()
    {
        background.sprite = ImageManager.Inst.backgroundSprites[0];
        try
        {
            string str = File.ReadAllText(Application.dataPath + "/Resources/" + currentLevelName + ".json");
            Level level = JsonConvert.DeserializeObject<Level>(str);

            currentLevel = level;
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e);
            return;
        }
        MapInstantiate();
    }

    public void MapReset(string str)
    {
        currentLevelName = str;
        MapReset();
    }

    public void MapReset(int stage, int level)
    {
        MapReset("Maps/Stage" + GameManager.Inst.stage + "/" + GameManager.Inst.stage + "-" + GameManager.Inst.level);
    }

    public void MapInstantiate()
    {
        CellInstantiate();
        RuleInstantiate();
        PaletteInstantiate();
        MapScale(7);
    }

    private void OnMouseUp()
    {
        MapInstantiate();
    }

    // Start is called before the first frame update
    void Start()
    {
        MapReset("Maps/Stage" + GameManager.Inst.stage.ToString() + "/" + GameManager.Inst.stage.ToString() + "-" + GameManager.Inst.level.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
