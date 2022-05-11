using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int power;

    public enum BulletType
    {
        Player,
        Enemy,
    }

    public BulletType myBullet;

    public Vector3 moveVec;
    public Vector3 changeVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (myBullet == BulletType.Player)
            moveVec = Vector3.up;
        else
            moveVec = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        if (changeVec == Vector3.zero)
            transform.Translate(moveVec * speed * Time.deltaTime);
        else
            transform.Translate(changeVec * speed * Time.deltaTime);
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Dead();
        }

        if(myBullet == BulletType.Player)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Dead();
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Dead();
            }
        }
    }
}
