using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    StageFlow stageFlow => StageFlow.Instance;
    GameManager gameManager => GameManager.Instance;

    public bool isBossesTime;

    [SerializeField] GameObject[] bossPrefabs;

    [Header("Boss")]
    public GameObject boss;
    public bool isBossTime;

    [Header("MiniBoss")]
    public GameObject mini1;
    public GameObject mini2;
    public bool isMiniTime;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckBossDead();
        CheckMiniBossDead();

        if(boss == null && mini1 == null && mini2 == null)
            UIController.Instance.ControllerBossHPUI(false);

    }

    public void SpawnBoss()
    {
        isBossesTime = true;
        isBossTime = true;
        boss = Instantiate(bossPrefabs[stageFlow.stage - 1]);

        Boss logic = boss.GetComponent<Boss>();
        SetBossMaxHP(logic);

        UIController.Instance.ShowBoss();
    }

    void SetBossMaxHP(Boss logic)
    {
        int num = 0;
        if (logic.myBoss == Boss.BossType.Boss)
        {
            switch (gameManager.mymode)
            {
                case GameManager.Mode.Easy:
                    num = 100;
                    break;
                case GameManager.Mode.Normal:
                    num = 200;
                    break;
                case GameManager.Mode.Hard:
                    num = 400;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (gameManager.mymode)
            {
                case GameManager.Mode.Easy:
                    num = 200;
                    break;
                case GameManager.Mode.Normal:
                    num = 400;
                    break;
                case GameManager.Mode.Hard:
                    num = 500;
                    break;
                default:
                    break;
            }
        }

        logic.MaxHP = num;
      
    }

    void CheckBossDead()
    {

        if (boss == null && isBossTime)
        {

            UIController.Instance.ControllerBossHPUI(false);
            isBossTime = false;
            if (stageFlow.isCheat)
            {
                stageFlow.isCheat = false;
                return;
            }


            SpawnMiniBoss();

        }


    }

    void SpawnMiniBoss()
    {
        isMiniTime = true;
        mini1 = Instantiate(bossPrefabs[stageFlow.stage + 1]);
        mini2 = Instantiate(bossPrefabs[stageFlow.stage + 1]);

        Boss logic1 = mini1.GetComponent<Boss>();
        Boss logic2 = mini2.GetComponent<Boss>();
        SetMiniBossMaxHP(logic1);
        SetMiniBossMaxHP(logic2);
        logic1.changeVec = Vector3.left;
        logic2.changeVec = Vector3.right;

        UIController.Instance.MaxMiniBossHP = logic1.MaxHP + logic2.MaxHP;
    }

    void SetMiniBossMaxHP(Boss logic)
    {
        int num = 0;
        if (logic.myBoss == Boss.BossType.Boss)
        {
            switch (gameManager.mymode)
            {
                case GameManager.Mode.Easy:
                    num = 50;
                    break;
                case GameManager.Mode.Normal:
                    num = 100;
                    break;
                case GameManager.Mode.Hard:
                    num = 200;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (gameManager.mymode)
            {
                case GameManager.Mode.Easy:
                    num = 100;
                    break;
                case GameManager.Mode.Normal:
                    num = 200;
                    break;
                case GameManager.Mode.Hard:
                    num = 250;
                    break;
                default:
                    break;
            }
        }

        logic.MaxHP = num;

    }

    void CheckMiniBossDead()
    {
        if (mini1 == null && mini2 == null && isMiniTime)
        {
            UIController.Instance.ControllerBossHPUI(false);
            isMiniTime = false;


            if (stageFlow.isCheat)
            {
                stageFlow.isCheat = false;
                return;
            }

            NextLevel();
        }


    }

    void NextLevel()
    {
        stageFlow.EndStage();
    }
}
