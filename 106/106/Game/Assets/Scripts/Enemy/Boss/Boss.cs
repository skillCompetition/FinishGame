using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public int MaxHP;
    public enum BossType
    {
        Boss,
        BossPlus,
    }

    public BossType myBoss;

    public GameObject[] enemies;
    public Transform[] spawnPos;

    private void Start()
    {
        HP = MaxHP;
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        col.enabled = false;
        yield return new WaitForSeconds(1f);
        speed = 0;
        col.enabled = true;
        Attackchose();

    }


    int attackIndex;
    void Attackchose()
    {
        if (attackIndex > 3)
            attackIndex = 0;
        switch (attackIndex)
        {
            case 0:
                StartCoroutine(CircleAttack());
                break;
                case 1:
                if (myBoss == BossType.Boss)
                {
                    StartCoroutine(SnakeAttack());

                }
                else
                {
                    StartCoroutine(Goto_Circle());

                }
                break;
            case 2:
                StartCoroutine(SpawnEnemy());
                break;
            case 3:
                StartCoroutine(TargetPlayer());
                break;
            default:
                break;
        }
    }

    IEnumerator CircleAttack()
    {
        int loopNum = 0;
        if (myBoss == BossType.Boss)
            loopNum = 3;
        else
            loopNum = 10;

        for (int i = 0; i < loopNum; i++)
        {
            for (int j = 0; j < 360; j += 13)
            {
                GameObject Bullet = Instantiate(base.bullet, transform.position, transform.rotation);
                Bullet.GetComponent<Bullet>().power = power;
                Bullet.transform.rotation = Quaternion.Euler(0, 0, j);
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
        attackIndex++;
        Attackchose();
    }

    IEnumerator SnakeAttack()
    {
        int bulletNum = 101;
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject Bullet = Instantiate(base.bullet, transform.position, transform.rotation);
            Bullet logic  = Bullet.GetComponent<Bullet>();
            logic.power = power;
            logic.changeVec =
                new Vector2(Mathf.Sin(Mathf.PI * 10 * i / bulletNum), -1);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        attackIndex++;
        Attackchose();

    }

    IEnumerator SpawnEnemy()
    {
        int loopNum = 0;
        if (myBoss == BossType.Boss)
            loopNum = 3;
        else
            loopNum = 5;
        for (int i = 0; i < loopNum; i++)
        {
            Transform t = spawnPos[Random.Range(0, spawnPos.Length)];

            Instantiate(enemies[Random.Range(0, enemies.Length)], t.position, t.rotation);

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
        attackIndex++;
        Attackchose();
    }

    IEnumerator Goto_Circle()
    {
        if (myBoss == BossType.Boss)
        {
            attackIndex++;
            Attackchose();
            yield break;

        }


        for (int i = 0; i < 3; i++)
        {
            List<Transform> b1 = new List<Transform>();

            for (int j = 0; j < 360; j+=13)
            {

                GameObject Bullet = Instantiate(base.bullet, transform.position, transform.rotation);
                Bullet.GetComponent<Bullet>().power = power;
                Bullet.transform.rotation = Quaternion.Euler(0, 0, j);

                b1.Add(Bullet.transform);
            }

            StartCoroutine(Go(b1));

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
        attackIndex++;
        Attackchose();
    }

    IEnumerator Go(List<Transform> b1)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(b1.Count);
        for (int i = 0; i < b1.Count; i++)
        {
            if(b1[i] == null)
                continue;

            Vector3 vec = player.transform.position - b1[i].position;

            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

            b1[i].rotation = Quaternion.AngleAxis(angle + 90 ,Vector3.forward);
        }
        b1.Clear();
    }

    IEnumerator TargetPlayer()
    {
        int bulletNum = 0;
        if (myBoss == BossType.Boss)
            bulletNum = 50;
        else
            bulletNum = 100;
        for (int i = 0; i < bulletNum; i++)
        {
            Vector3 vec = player.transform.position - transform.position;

            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

            GameObject Bullet = Instantiate(base.bullet, transform.position, transform.rotation);
            Bullet.GetComponent<Bullet>().power = power;

            Bullet.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
        attackIndex++;
        Attackchose();
    }
}
