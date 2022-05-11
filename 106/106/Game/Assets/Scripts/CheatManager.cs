using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatManager : Singleton<CheatManager>
{
    GameManager gameManager => GameManager.Instance;
    StageFlow stageFlow => StageFlow.Instance;
    Player player => Player.Instance;
    SpawnManager spawnManager => SpawnManager.Instance;
    

    [Header("UI")]
    [SerializeField] GameObject cheatPanel;
    [SerializeField] Slider hpSlider;
    [SerializeField] Text hpText;
    [SerializeField] Slider painSlider;
    [SerializeField] Text painText;


    void Start()
    {
        cheatPanel.SetActive(false);
    }

    void Update()
    {
        MoveStage1();
        MoveStage2();
        BulletLevelPlus();
        BulletLevelMinus();
        AllEnemyDeadCheck();
        ShowCheatPanel();
        SpawnRed();
        SpawnWhite();
        GodOn();
        GodOff();
    }


    void MoveStage1()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {

            spawnManager.StopAllCoroutines();
            stageFlow.MoveStage(1);
        }
    }

    void MoveStage2()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            spawnManager.StopAllCoroutines();
            stageFlow.MoveStage(2);
        }
    }

    void BulletLevelPlus()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.BulletLevel++;
        }
    }

    void BulletLevelMinus()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            player.BulletLevel--;
        }
    }

    void GodOn()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (player.godCoroutine != null)
                player.StopCoroutine(player.godCoroutine);
            player.isGod = true;
            player.spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        }
    }

    void GodOff()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if(player.godCoroutine != null)
                player.StopCoroutine(player.godCoroutine);
            player.isGod = false;
            player.spriteRenderer.color = Color.white;

        }
    }

    void AllEnemyDeadCheck()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            AllEnemyDead(true);
        }
    }

    public void AllEnemyDead(bool isCheat)
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (!isCheat)
            {
                if (enemy.isBoss)
                {
                    continue;
                }
            }

            enemy.Dead();
        }
    }


    void ShowCheatPanel()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            cheatPanel.SetActive(true);
            ControllerCheatPanel();
        }
        hpText.text = ((int)hpSlider.value).ToString();
        painText.text = ((int)painSlider.value).ToString();
    }

    void ControllerCheatPanel()
    {
        hpSlider.value = gameManager.HP;
        painSlider.value = gameManager.Pain;
    }

    public void CheatPanelBtnClick()
    {
        gameManager.HP = (int)hpSlider.value;
        gameManager.Pain = (int)painSlider.value;
        cheatPanel.SetActive(false);
    }

    public void SpawnRed()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            spawnManager.SpawnRed();
        }
    }
    
    public void SpawnWhite()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            spawnManager.SpawnWhite();
        }
    }
}
