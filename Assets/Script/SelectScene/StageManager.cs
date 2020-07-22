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

    public void Start()
    {
        for (int i = 0; i < stage_count; i++)
        {
            GameObject created_instance = Instantiate(prefStageSelectButton, objStageSelectScreen.transform);
            created_instance.GetComponent<Transform>().position = new Vector2(-5 + (i % 3) * 5, -2.5f - (i / 3));
            created_instance.GetComponent<StageSelectButton>().selected_stage = i + 1;
        }
    }

    public void StageSelected(int n)
    {
        objStageSelectScreen.SetActive(false);
        setLevelSelectScreen(n);
    }

    public void BackSelected()
    {
        objLevelSelectScreen.SetActive(false);
        objStageSelectScreen.SetActive(true);
    }

    void setLevelSelectScreen(int n)
    {
        objLevelSelectScreen.SetActive(true);
        for (int i = 0; i < level_count[n - 1]; i++)
        {
            GameObject created_instance = Instantiate(prefLevelSelectButton, objLevelSelectScreen.transform);
            created_instance.GetComponent<Transform>().position = new Vector2(-6 + (i % 5) * 3, 3 - (i / 5));
            created_instance.GetComponent<LevelSelectButton>().selected_level = i + 1;

            GameObject created_instance_back_button = Instantiate(prefBackSelectButton, objLevelSelectScreen.transform);
            created_instance_back_button.GetComponent<Transform>().position = new Vector2(0, -4);
        }
    }
}
