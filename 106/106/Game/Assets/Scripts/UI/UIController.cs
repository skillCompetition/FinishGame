using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
    GameManager gameManager => GameManager.Instance;
    BossManager bossManager => BossManager.Instance;

    [Header("Player")]
    [SerializeField] Text scoreText;
    [SerializeField] Image hpImg;
    [SerializeField] Text hpText;
    [SerializeField] Image painImg;
    [SerializeField] Text painText;


    [Header("BossHP")]
    [SerializeField] Image bossHpImage;
    [SerializeField] Text bossHpText;
    public float MaxMiniBossHP;

    [Header("BossShow")]
    [SerializeField] GameObject bossWarningImg;
    Animator bossWarningAnim;
    AudioSource bossWarningAudioSource;

    [Header("Stop")]
    [SerializeField] GameObject stopPanel;

    [Header("Item")]
    [SerializeField] Image itemImg;
    [SerializeField] Text itemText;

    public override void Awake()
    {
        base.Awake();
        bossWarningAnim = bossWarningImg.GetComponent<Animator>();
        bossWarningAudioSource = bossWarningImg.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        stopPanel.SetActive(false);
        ControllerBossHPUI(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerState();
        BossState();
    }

    void PlayerState()
    {
        if (gameManager.HP < 0 || gameManager.Pain > GameManager.MaxPain)
            return;

        scoreText.text = "Score: " + gameManager.enemyScore.ToString();
        hpImg.fillAmount = (float)gameManager.HP / GameManager.MaxHP;
        hpText.text = ((int)gameManager.HP).ToString();
        painImg.fillAmount = (float)gameManager.Pain / GameManager.MaxPain;
        painText.text = ((int)gameManager.Pain).ToString();
    }

    void BossState()
    {
        if (bossManager.boss != null && bossManager.isBossTime == true)
        {
            ControllerBossHPUI(true);
            Boss boss = bossManager.boss.GetComponent<Boss>();
            if (boss.HP < 0)
                return;
            bossHpImage.fillAmount = (float)(boss.HP) / boss.MaxHP;
            bossHpText.text = boss.HP.ToString() + "/" + boss.MaxHP.ToString();
        }
        else if(bossManager.isMiniTime == true)
        {
            ControllerBossHPUI(true);
            MiniBossState();
        }
    }

    void MiniBossState()
    {
        if (bossManager.mini1 != null && bossManager.mini2 != null)
        {
            Boss logic1 = bossManager.mini1.GetComponent<Boss>();
            Boss logic2 = bossManager.mini2.GetComponent<Boss>();

            bossHpImage.fillAmount = (float)(logic1.HP + logic2.HP) / MaxMiniBossHP;
            bossHpText.text = (logic1.HP + logic2.HP).ToString() + "/" + MaxMiniBossHP.ToString();
        }
        else if(bossManager.mini1 == null && bossManager.mini2 != null)
        {
            Boss logic2 = bossManager.mini2.GetComponent<Boss>();
            if (logic2.HP < 0)
                return;
            bossHpImage.fillAmount = (float)logic2.HP / MaxMiniBossHP;
            bossHpText.text = logic2.HP.ToString() + "/" + MaxMiniBossHP.ToString();
        }
        else if(bossManager.mini1 != null && bossManager.mini2 == null)
        {
            Boss logic1 = bossManager.mini1.GetComponent<Boss>();
            if (logic1.HP < 0)
                return;
            bossHpImage.fillAmount =(float)logic1.HP / MaxMiniBossHP;
            bossHpText.text = logic1.HP.ToString() + "/" + MaxMiniBossHP.ToString();

        }
    }

    public void ControllerBossHPUI(bool isShow)
    {
        bossHpImage.enabled = isShow;
        bossHpText.enabled = isShow;
    }

    public void ShowBoss()
    {
        bossWarningAudioSource.Play();
        bossWarningAnim.SetTrigger("isShow");
    }

    public void ShowStopPanel()
    {
        stopPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueBtnClick()
    {
        Time.timeScale = 1;
        stopPanel.SetActive(false);
    }

    public void ExitBtnClcik()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");

    }

    public void SetItemUse(Item.ItemType myItem)    {
        switch (myItem)
        {
            case Item.ItemType.Power:
                itemImg.color = Color.green;
                itemText.text = "Power Up";
                break;
            case Item.ItemType.God:
                itemImg.color = Color.yellow;
                itemText.text = "God";
                break;
            case Item.ItemType.HP:
                itemImg.color = Color.blue;
                itemText.text = "HP Up";
                break;
            case Item.ItemType.Pain:
                itemImg.color = Color.red;
                itemText.text = "Pain Down";
                break;
            case Item.ItemType.Boom:
                itemImg.color = Color.magenta;
                itemText.text = "Boom";
                break;
            case Item.ItemType.Follow:
                itemImg.color = Color.cyan;
                itemText.text = "Follow";
                break;
            default:
                itemImg.color = new Color(1, 1, 1, 0);
                itemText.text = "";
                break;
        }
    }
}
