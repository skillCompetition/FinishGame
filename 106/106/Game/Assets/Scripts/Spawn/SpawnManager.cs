using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnManager : Singleton<SpawnManager>
{
    GameManager gameManager => GameManager.Instance;
    BossManager bossManager => BossManager.Instance;

    List<Spawn> spawnList = new List<Spawn>();
    [SerializeField] GameObject[] enemies;
    [SerializeField] Transform[] spawnPoses;      //6°³

    public Coroutine spawnCoroutine;

    [Header("Red")]
    [SerializeField] GameObject redPrefab;
    [SerializeField] Transform[] redPos;
    [SerializeField] float redDelay;
    [SerializeField] int redran;
    float redTimer;

    [Header("White")]
    [SerializeField] GameObject whitePrefab;
    [SerializeField] Transform[] whitePos;
    [SerializeField] float whiteDelay;
    [SerializeField] int whiteran;
    float whiteTimer;


    // Update is called once per frame
    void Update()
    {
        RedSpawnCheck();
        WhiteSpawnCheck();
    }

    public void SpawnDataRead(int stage)
    {
        spawnList.Clear();
        string name = "stage" + stage + "_" + gameManager.mymode;
        TextAsset textAsset = Resources.Load(name) as TextAsset;
        StringReader stringReader = new StringReader(textAsset.text);
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            if (line == null)
                break;
            Spawn spawn = new Spawn();
            spawn.name = line.Split(',')[0];
            spawn.pos = int.Parse(line.Split(',')[1]);
            spawn.delay = float.Parse(line.Split(',')[2]);

            spawnList.Add(spawn);

        }
        stringReader.Close();
        spawnCoroutine =  StartCoroutine(SpawnEnemy(spawnList));

    }

    IEnumerator SpawnEnemy(List<Spawn> list)
    {

        for (int i = 0; i < list.Count; i++)
        {
            GameObject enemy = ReturnEnemy(list[i].name);
            Transform t = spawnPoses[list[i].pos];
            Instantiate(enemy,t.position,t.rotation);

            yield return new WaitForSeconds(list[i].delay);
        }
        spawnList.Clear();

        yield return new WaitForSeconds(2f);

        bossManager.SpawnBoss();
    }

    GameObject ReturnEnemy(string name)
    {
        GameObject enemy = null;
        switch (name)
        {
            case "B":
                enemy = enemies[0];
                break;
            case "G":
                enemy = enemies[1];
                break;
            case "V":
                enemy = enemies[2];
                break;
            case "C":
                enemy = enemies[3];
                break;

            default:
                break;
        }

        return enemy;
    }

    void RedSpawnCheck()
    {
        redTimer += Time.deltaTime;
        if (redTimer >= redDelay)
        {
            if (Random.Range(0,redran) == 0)
            {
                SpawnRed();
            }
            redTimer = 0;
        }
    }

    public void SpawnRed()
    {
        Transform t = redPos[Random.Range(0,redPos.Length)];
        Instantiate(redPrefab, t.position, t.rotation);
    }
    
    void WhiteSpawnCheck()
    {
        whiteTimer += Time.deltaTime;
        if (whiteTimer >= redDelay)
        {
            if (Random.Range(0,whiteran) == 0)
            {
                SpawnWhite();
            }
            whiteTimer = 0;
        }
    }

    public void SpawnWhite()
    {
        Transform t = whitePos[Random.Range(0,whitePos.Length)];
        Instantiate(whitePrefab, t.position, t.rotation);
    }

    
}
