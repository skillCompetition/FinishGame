using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIController : MonoBehaviour
{
    AudioSource audioSource;

    [Header("MiniPlayer")]
    [SerializeField] GameObject miniPayer;
    Animator anim;
    AudioSource miniAudio;

    [Header("Information")]
    [SerializeField] GameObject informationPanel;
    [SerializeField] GameObject[] inforTexts;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = miniPayer.GetComponent<Animator>();
        miniAudio = miniPayer.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        informationPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBtnClick()
    {
        StartCoroutine(StartGame());
    }

    public void InformationBtnClick()
    {
        informationPanel.SetActive(!informationPanel.activeSelf);
    }

    public void StroyBtnClick()
    {
        for (int i = 0; i < inforTexts.Length; i++)
        {
            if (i == 0)
                inforTexts[i].SetActive(true);
            else
                inforTexts[i].SetActive(false);
        }
    }

    public void KeyButtonClick()
    {
        for (int i = 0; i < inforTexts.Length; i++)
        {
            if (i == 1)
                inforTexts[i].SetActive(true);
            else
                inforTexts[i].SetActive(false);
        }
    }

    public void MonsterBtnClick()
    {
        for (int i = 0; i < inforTexts.Length; i++)
        {
            if (i == 2)
                inforTexts[i].SetActive(true);
            else
                inforTexts[i].SetActive(false);
        }
    }

    public void ItemBtnClick()
    {
        for (int i = 0; i < inforTexts.Length; i++)
        {
            if (i == 3)
                inforTexts[i].SetActive(true);
            else
                inforTexts[i].SetActive(false);
        }
    }

    public void CheatInforBtnClick()
    {
        for (int i = 0; i < inforTexts.Length; i++)
        {
            if (i == 4)
                inforTexts[i].SetActive(true);
            else
                inforTexts[i].SetActive(false);
        }
    }

    IEnumerator StartGame()
    {
        anim.SetTrigger("isStart");
        miniAudio.Play();
        yield return new WaitForSeconds(2.3f);
        SceneManager.LoadScene("GameScene");

    }
}
