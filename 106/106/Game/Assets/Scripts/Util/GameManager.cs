using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    Player player => Player.Instance;

    public const float MaxHP = 100f;
    [SerializeField] float hp;
    public float HP { 
        get { return hp; } 
        set { hp = value; 
            if(hp < 0 && !isGameOver)
            {
                hp = 0;
                Dead();
            }
            else if(hp >= MaxHP)
                hp = MaxHP;
        }
    }
    public const float MaxPain = 100f;
    [SerializeField] float pain;
    public float Pain
    {
        get { return pain; }
        set
        {
            pain = value;
            if (pain >= MaxPain && !isGameOver)
            {
                pain = MaxPain;
                Dead();
            }
            else if (pain <= 0)
                pain = 0;
        }
    }

    public bool isGameOver;

    [Header("Score")]
    public int totalScore;
    public int enemyScore;
    public int itemScore;
    public int stageScore;

    public enum Mode
    {
        Easy,
        Normal,
        Hard
    }
    public Mode mymode;

    public List<Rank> rankingList = new List<Rank>();



    void Dead()
    {
        player.Dead();

        Invoke("GameOver", 1f);
    }

    public void GameOver()
    {

        totalScore = enemyScore + itemScore + stageScore;

        isGameOver = true;

        SceneManager.LoadScene("EndScene");
    }

    public void Init(int stage)
    {
        isGameOver =false;
        switch (stage)
        {
            case 1:
                Debug.Log("dfd");
                totalScore = 0;
                enemyScore = 0;
                itemScore  = 0;
                stageScore = 0;
                player.transform.position = new Vector3(0,-8,0);
                pain = (int)(MaxPain * 0.1f);
                player.StartCoroutine(player.StartShow());

                break;
            case 2:
                player.transform.position = new Vector3(0,-2,0);
                pain = (int)(MaxPain * 0.3f);
                break;
            default:
                break;
        }

        HP = MaxHP;
    }
}
