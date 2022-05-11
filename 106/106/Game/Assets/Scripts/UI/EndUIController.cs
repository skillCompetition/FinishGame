using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndUIController : Singleton<EndUIController>
{
    GameManager gameManager => GameManager.Instance;

    [Header("Score")]
    [SerializeField] Text[] scoreTexts;

    AudioSource audioSource;

    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
    }

    private void Update()
    {

    }



    public void RestartBtnClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GameOverBtnClcik()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void StartShowText()
    {
        StartCoroutine(ShowText(scoreTexts));

    }

    public IEnumerator ShowText(Text[] texts)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < texts.Length; i++)
        {
            audioSource.Play();
            switch (i)
            {
                case 0:
                    texts[i].text = gameManager.enemyScore.ToString();
                    break;
                case 1:
                    texts[i].text = gameManager.itemScore.ToString();
                    break;
                case 2:
                    texts[i].text = gameManager.stageScore.ToString();
                    break;
                case 3:
                    StartCoroutine(TotalScoreEffect(texts[i]));
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1f);
        }

        RankingManager.Instance.StartShowRanking();

    }

    IEnumerator TotalScoreEffect(Text text)
    {
        while (true)
        {
            text.text = gameManager.totalScore.ToString();
            yield return new WaitForSeconds(0.5f);
            text.text = "";
            yield return new WaitForSeconds(0.5f);

        }

    }

}
