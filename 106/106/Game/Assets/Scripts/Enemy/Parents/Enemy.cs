using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Player player => Player.Instance;

    [SerializeField] int hp;
    public int HP
    {
        get { return hp; }  
        set {
            hp = value;
            if (hp < 0 && isDead == false)
            {
                hp = 0;
                Dead();

            }
        }
    }
    public float speed;
    public int power;
    public int score;
    public bool isDead;
    public bool isBoss;

    Vector3 moveVec = Vector3.down;
    public Vector3 changeVec = Vector3.zero;

    [SerializeField] protected GameObject bullet;
    Animator anim;
    protected Collider2D col;
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] clips;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (changeVec == Vector3.zero)
            transform.Translate(moveVec * speed * Time.deltaTime);
        else
            transform.Translate(changeVec * speed * Time.deltaTime);
    }

    void OnHit(int damage)
    {
        if (HP < 0)
            return;
        anim.SetTrigger("OnHit");
        HP -= damage;
    }

    public void Dead()
    {
        player.warpSlider.value += 5;
        StopAllCoroutines();
        speed = 0;
        audioSource.clip = clips[1];
        audioSource.Play();
        col.enabled = false;
        GameManager.Instance.enemyScore += score;
        isDead = true;
        anim.SetTrigger("OnDead");
        Destroy(gameObject, 2f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (isBoss)
                return;
            GameManager.Instance.Pain += (int)(power * 0.5);
            Destroy();
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            if (bullet.myBullet == Bullet.BulletType.Player)
            {
                OnHit(bullet.power);
            }
        }
    }
}
