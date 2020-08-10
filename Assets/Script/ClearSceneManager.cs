using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;


public class ClearSceneManager : MonoBehaviour
{
    public GameObject ClearWindow;

    public void ChangeSelectScene()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void ChangeNextLevel()
    {
        ClearWindow.SetActive(false);

        if (GameManager.Inst.level == 10)
        {
            GameManager.Inst.stage += 1;
            GameManager.Inst.level = 1;
        }

        else
        {
            GameManager.Inst.level += 1;
        }

        LevelManager.Inst.MapReset(GameManager.Inst.stage, GameManager.Inst.level);
        LevelManager.Inst.SetPlayState(PlayState.EDIT);
    }

    public void ChangeReplay()
    {
        ClearWindow.SetActive(false);

        LevelManager.Inst.MapReset(GameManager.Inst.stage, GameManager.Inst.level);
        LevelManager.Inst.SetPlayState(PlayState.EDIT);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
