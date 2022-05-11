using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageFlow : Singleton<StageFlow>
{
    SpawnManager spawnManager => SpawnManager.Instance;
    GameManager gameManager => GameManager.Instance;
    BossManager bossManager => BossManager.Instance;
    Player player => Player.Instance;

    public int stage = 1;
    public bool isStaeClear = false;
    public bool isCheat;

    [Header("BackgroundSound")]
    AudioSource backgroundMusic;

    [Header("StageAnim")]
    [SerializeField] Image stageImg;
    [SerializeField] Text stageText;
    Animator stageAnim;
    AudioSource stageAudio;

    [Header("StageClearAnim")]
    [SerializeField] GameObject stageClearPanel;
    Animator stageClearAnim;
    AudioSource stageClearAudio;

    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip[] clips;

    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        stageAnim = stageImg.GetComponent<Animator>();
        stageAudio = stageImg.GetComponent<AudioSource>();
        stageClearAnim = stageClearPanel.GetComponent<Animator>();
        stageClearAudio = stageClearPanel.GetComponent<AudioSource>();
    }

    void Start()
    {
        stageClearPanel.SetActive(false);
        StartStage();
    }

    void Update()
    {
        
    }

    public void StartStage()
    {
        if (stage > 2)
        {
            EndStage();
            return;
        }

        PlayBGM(stage);
        UIController.Instance.ControllerBossHPUI(false);
        gameManager.Init(stage);
        Background.Instance.ChangeBackground(stage);
        stageText.text = "stage" +  stage.ToString();
        stageAudio.Play();
        stageAnim.SetTrigger("StartStage");
        spawnManager.SpawnDataRead(stage);

        for (int i = 0; i < player.followers.Length; i++)
        {
            player.followers[i].SetActive(false);
        }

    }

    public void PlayBGM(int stage)
    {
        switch (stage)
        {
            case 1:
                audioSource.clip = clips[0];
                break;
            case 2:
                audioSource.clip = clips[1];
                break;
            default:
                break;
        }
        audioSource.Play();

    }

    public void EndStage()
    {
        if(stage > 2)
        {
            StartCoroutine(ClearCoroutine());
        }
        else
        {
            gameManager.stageScore += (int)(gameManager.HP + (GameManager.MaxPain - gameManager.Pain));
            stage++;
            StartStage();
        }
    }

    IEnumerator ClearCoroutine()
    {

        stageClearPanel.SetActive(true);

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Dead();
        }
        Bullet[] bullets = GameObject.FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            bullet.Dead();
        }
        NPC[] npcs = GameObject.FindObjectsOfType<NPC>();
        foreach (NPC npc in npcs)
        {
            npc.Destroy();
        }
        Item[] items = GameObject.FindObjectsOfType<Item>();
        foreach (Item item in items)
        {
            item.Dead();
        }

        isStaeClear = true;
        stageClearAnim.SetTrigger("isClear");
        stageClearAudio.Play();
        yield return new WaitForSeconds(3f);
        gameManager.GameOver();
    }

    public void MoveStage(int stage)
    {
        this.stage = stage;

        if (bossManager.isBossesTime)
        {
            isCheat = true;
        }

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Destroy();
        }
        Bullet[] bullets = GameObject.FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            bullet.Dead();
        }
        NPC[] npcs = GameObject.FindObjectsOfType<NPC>();
        foreach (NPC npc in npcs)
        {
            npc.Destroy();
        }
        Item[] items = GameObject.FindObjectsOfType<Item>();
        foreach(Item item in items)
        {
            item.Dead();
        }
        StartStage();
    }
}
