using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;

    public float speed;
    [SerializeField] GameObject[] items;

    public enum NPCType
    {
        Red,
        White,
    }

    public NPCType myNPC;

    void Start()
    {
        StartCoroutine(Move());
    }

    void Update()
    {
        
    }

    IEnumerator Move()
    {
        while (true)
        {
            float dir = Random.Range(-1, 2);
            float timer = 0;
            while (true)
            {
                timer += Time.deltaTime;
                transform.Translate(new Vector3(dir, -1, 0) * speed * Time.deltaTime);
                if (timer > 0.5f)
                    break;
                yield return new WaitForEndOfFrame();
            }

            timer = 0;
        }
    }

    void Use()
    {
        if (myNPC == NPCType.Red)
        {
            gameManager.Pain += 5f;
            
        }
        else
        {
           GameObject item =  Instantiate(items[Random.Range(0, items.Length)],transform.position,transform.rotation);
           item.transform.rotation = Quaternion.Euler(0,0,90);

        }

        Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            
            Use();
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.gameObject.GetComponent<Bullet>().myBullet == Bullet.BulletType.Player)
            {
                Use();
            }
        }

        if (myNPC == NPCType.Red)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                if (collision.gameObject.GetComponent<Bullet>().myBullet == Bullet.BulletType.Enemy)
                {
                    Use();
                }
            }
        }
    }
}
