using System.Collections;
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

    /*
     LevelEditor와 관련된 변수들.
     isTestMode는 LevelEditor의 test 버튼을 통하여 넘어왔는지를 저장함.
     LevelEditor<->TestScene간의 이동 동안에는 항상 true 값이 저장되어 있어야 함.
     LevelEditor에서 TestScene이 아닌 다른 Scene으로의 이동일 경우에는 false 값이 저장되어야 함.
     back은 TestScene의 back 버튼을 통하여 넘어왔는지를 저장함.
     이를 통하여 LevelEditor에서는 test.json을 불러들이게 되며, LevelEditor에서 다시 back flag를 false로 지정하도록 함.
     */
    public bool isTestMode, back;
    public int editNum;

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

    public void TestLevel()
    {
        isTestMode = true;
        back = false;
        SceneManager.LoadScene("TestScene");
    }

    public void BackToLevelEditor()
    {
        isTestMode = true;
        back = true;
        SceneManager.LoadScene("LevelEditor");
    }

    public void LoadPuzzle(int StageToGo, int LevelToGo)
    {
        stage = StageToGo;
        level = LevelToGo;
        SceneManager.LoadScene("PuzzleScene");
    }

    public void LoadCustomPuzzle(int LevelToGo)
    {
        stage = -1;
        level = LevelToGo;
        SceneManager.LoadScene("PuzzleScene");
    }

    public void ClearPuzzle(int SelectedStage, int SelectedLevel)
    {
        if (!isTestMode)
        {
            LevelClearData.IsClear[SelectedStage - 1, SelectedLevel - 1] = true;
            SaveLevelClearData();
        }
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
