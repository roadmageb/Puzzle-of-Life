using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
