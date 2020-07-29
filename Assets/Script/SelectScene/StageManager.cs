using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public int stage_count;
    public int[] level_count;
    public GameObject objStageSelectScreen;
    public GameObject objLevelSelectScreen;
    public GameObject prefStageSelectButton;
    public GameObject prefLevelSelectButton;
    public GameObject prefBackSelectButton;

    int selected_stage;
    int selected_level;
    List<GameObject> LevelSelectScreen_LevelSelectButton_List;

    public void Start()
    {
        for (int i = 0; i < stage_count; i++)
        {
            GameObject created_instance = Instantiate(prefStageSelectButton, objStageSelectScreen.transform);
            created_instance.GetComponent<Transform>().position = new Vector2(-5 + (i % 3) * 5, -2.5f - (i / 3));
            created_instance.GetComponent<StageSelectButton>().stage_to_go = i + 1;
        }
    }

    public void StageSelected(int n)
    {
        objStageSelectScreen.SetActive(false);
        selected_stage = n;
        setLevelSelectScreen();
    }

    public void BackSelected()
    {
        closeLevelSelctScreen();
        objStageSelectScreen.SetActive(true);
    }

    void setLevelSelectScreen()
    {
        objLevelSelectScreen.SetActive(true);
        LevelSelectScreen_LevelSelectButton_List = new List<GameObject>();
        for (int i = 0; i < level_count[selected_stage - 1]; i++)
        {
            GameObject created_instance = Instantiate(prefLevelSelectButton, objLevelSelectScreen.transform);
            created_instance.GetComponent<Transform>().position = new Vector2(-3 + (i % 5) * 1.5f, 3 - (i / 5) * 1.5f);
            created_instance.GetComponent<LevelSelectButton>().level_to_go = i + 1;
            LevelSelectScreen_LevelSelectButton_List.Add(created_instance);
        }
    }

    void closeLevelSelctScreen()
    {
        for (int i = 0; i < level_count[selected_stage - 1]; i++)
        {
            Destroy(LevelSelectScreen_LevelSelectButton_List[i].gameObject);
        }
        objLevelSelectScreen.SetActive(false);
    }
}
