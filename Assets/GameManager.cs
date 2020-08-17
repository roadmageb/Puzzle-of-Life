﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;

public class GameManager : Singleton<GameManager>
{
    //총 스테이지의 개수와 각 스테이지별 레벨의 개수를 담는 변수들입니다.
    //레벨의 수에 변동이 생길 때 마다 인스펙터에서 이 변수들의 값을 바꿔주시면 됩니다.
    public int StageCount;//총 스테이지의 수입니다.
    public int[] LevelCount;//각 스테이지의 레벨 수입니다. 인스펙터에서 StageCount의 수와 LevelCount 사이즈를 같게 해 주어야 합니다.

    //선택된 맵과 관련된 변수입니다.
    //"Stage2-3"으로 예를 들면 '2'는 stage, '3'은 level입니다.
    public int stage;//선택된 스테이지 번호입니다.
    public int level;//선택된 레벨 번호입니다.

    public LevelClearDataStruct LevelClearData;
    string LevelClearDataPath;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        LevelClearDataPath = Path.Combine(Application.persistentDataPath + "/LevelClearData.json");
        FileInfo savefile = new FileInfo(LevelClearDataPath);
        if (savefile.Exists)
        {
            LoadLevelClearData();
        }
        else
        {
            LevelClearData.IsClear = new bool[StageCount, 10];
            for (int i = 0; i < StageCount; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    LevelClearData.IsClear[i, j] = false;
                }
            }
            SaveLevelClearData();
        }
    }

    void SaveLevelClearData()
    {
        string jsonData = JsonConvert.SerializeObject(LevelClearData);
        File.WriteAllText(LevelClearDataPath, jsonData);
    }

    void LoadLevelClearData()
    {
        string jsonData = File.ReadAllText(LevelClearDataPath);
        LevelClearData = JsonConvert.DeserializeObject<LevelClearDataStruct>(jsonData);
    }

    public void LoadPuzzle(int StageToGo, int LevelToGo)
    {
        stage = StageToGo;
        level = LevelToGo;
        SceneManager.LoadScene("PuzzleScene");
    }

    public void ClearPuzzle(int SelectedStage, int SelectedLevel)
    {
        LevelClearData.IsClear[SelectedStage - 1, SelectedLevel - 1] = true;
        SaveLevelClearData();
    }

    public bool IsCleared(int IdentifiedStage, int IdentifiredLevel)
    {
        return LevelClearData.IsClear[IdentifiedStage - 1, IdentifiredLevel - 1];
    }

    public bool IsPlayable(int IdentifiedStage, int IdentifiedLevel)
    {
        if (IsCleared(IdentifiedStage, IdentifiedLevel))
        {
            return true;
        }
        else
        {
            int last_stage = IdentifiedStage;
            int last_level = IdentifiedLevel - 1;
            if (last_level == 0)
            {
                last_stage -= 1;
                if (last_stage == 0)
                {
                    return true;
                }
                last_level = LevelCount[last_stage - 1];
            }
            return IsCleared(last_stage, last_level);
        }
    }
}

[System.Serializable]
public class LevelClearDataStruct
{
    public bool[,] IsClear;
    public LevelClearDataStruct()
    {
    }
}
