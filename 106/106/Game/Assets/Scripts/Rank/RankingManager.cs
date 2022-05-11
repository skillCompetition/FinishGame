using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : Singleton<RankingManager>
{
    GameManager gameManager => GameManager.Instance;

    List<Rank> rankingList => GameManager.Instance.rankingList;

    [Header("Ranking")]
    [SerializeField] Text[] nameTexts;
    [SerializeField] Text[] scoresTexts;

    [Header("UI")]
    [SerializeField] GameObject inputRankingPanel;
    AudioSource rankingAudio;
    [SerializeField] InputField input;
    [SerializeField] GameObject badName;

    string[] noName = { "¾¾¹ß", "Á¸³ª", "°³»õ³¢", "³ª»Û³ð","" };

    public override void Awake()
    {
        base.Awake();
        rankingAudio = inputRankingPanel.GetComponent<AudioSource>();
    }

    void Start()
    {
        inputRankingPanel.SetActive(false);
        badName.SetActive(false);
        if (rankingList.Count < 5 || rankingList[4].score <= gameManager.totalScore)
        {
            InputRanking();
        }
        else
        {
            StartShowRanking();
        }
    }

    void Update()
    {
        
    }

    void InputRanking()
    {
        rankingAudio.Play();
        inputRankingPanel.SetActive(true);

    }

    public void InputBtnClcik()
    {
        for (int i = 0; i < noName.Length; i++)
        {
            if (noName[i] == input.text)
            {
                badName.SetActive(true);
                return;
            }
        }

        Rank rank = new Rank();
        rank.name = input.text;
        rank.score = gameManager.totalScore;
        inputRankingPanel.SetActive(false);

        rankingList.Add(rank);

        RankingSet();
    }


    void RankingSet()
    {
        rankingList.Sort((rank1,rank2) => rank1.score.CompareTo(rank2.score));
        rankingList.Reverse();

        EndUIController.Instance.StartShowText();
    }



    public void StartShowRanking()
    {
        StartCoroutine(ShowRanking());
    }

    IEnumerator ShowRanking()
    {
        for (int i = 0; i < rankingList.Count; i++)
        {
            nameTexts[i].text = rankingList[i].name;
            scoresTexts[i].text = rankingList[i].score.ToString();

            yield return new WaitForSeconds(0.5f);
        }
    }
}
