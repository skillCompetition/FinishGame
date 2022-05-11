using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Singleton<Player>
{
    public float speed;


    [Header("Fire")]
    public GameObject[] bullets;
    [SerializeField] int bulletLevel;
    public int BulletLevel
    {
        get { return bulletLevel; }
        set { bulletLevel = value;
            if (bulletLevel >= 5)
                bulletLevel = 4;
            else if (bulletLevel < 0)
                bulletLevel = 0;
        }
    }
    float fireTimer;
    public float fireDelay;
    bool isDead;

    [Header("SuperShot")]
    [SerializeField] GameObject superShotBullet;
    [SerializeField] Slider superSlider;
    bool isSuperShot;

    [Header("Warp")]
    public Slider warpSlider;

    [Header("God")]
    public Coroutine godCoroutine;
    public bool isGod;

    [Header("Boom")]
    public int boomIndex;
    [SerializeField] Image[] boomImages;

    [Header("Follower")]
    public GameObject[] followers;

    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip[] clips;

    BoxCollider2D col;
    public SpriteRenderer spriteRenderer;
    Animator anim;

    public override void Awake()
    {
        base.Awake();
        col = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public IEnumerator StartShow()
    {
        col.enabled = false;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (timer > 1f)
                break;
            yield return new WaitForEndOfFrame();
        }
        col.enabled = true;
    }

    void Update()
    {
        FireCheck();
        SuperShotCheck();
        UseBoomCheck();
        WarpCheck();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        anim.SetInteger("isMove", (int)h);

        if (StageFlow.Instance.isStaeClear)
        {
            col.enabled = false;
            transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);

        }


        transform.Translate(new Vector3(h, v, 0) * speed * Time.fixedDeltaTime);

    }


    void FireCheck()
    {
        fireTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && fireTimer >= fireDelay && !isSuperShot)
        {
            Fire();
            fireTimer = 0;

        }
        else if(Input.GetMouseButtonUp(0))
        {
            audioSource.Stop();
        }
    }

    void Fire()
    {
        if (isDead)
            return;
        audioSource.clip = clips[0];
        audioSource.Play();
        Instantiate(bullets[bulletLevel], transform.position, transform.rotation);

    }

    void SuperShotCheck()
    {
        if (Input.GetMouseButton(1) && superSlider.value == 100)
        {
            StartCoroutine(SuperShot());
        }
    }


    IEnumerator SuperShot()
    {
        audioSource.clip = clips[1];
        audioSource.Play();
        audioSource.loop = true;
        isSuperShot = true;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            Instantiate(superShotBullet, transform.position, transform.rotation);
            superSlider.value -= 1;
            if (timer >= 0.5f)
            {
                isSuperShot = false;
                audioSource.Stop();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void Dead()
    {
        StopAllCoroutines();
        isDead = true;
        audioSource.clip = clips[2];
        audioSource.volume = 1f;
        audioSource.Play();
        anim.SetTrigger("isDead");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (isGod)
                return;
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            if (bullet.myBullet == Bullet.BulletType.Enemy)
            {
                GameManager.Instance.HP -= bullet.power;
                superSlider.value += 10;
                God(false);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isGod)
                return;
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            GameManager.Instance.HP -= (int)(enemy.power * 0.5f);
            God(false);
        }


    }

    public void ShowFollow()
    {
        for (int i = 0; i < followers.Length; i++)
        {
            if (!followers[i].activeSelf)
            {
                followers[i].gameObject.SetActive(true);
                return;
            }

        }
    }

    void WarpCheck()
    {
        if (Input.GetMouseButtonDown(1) && warpSlider.value >= 100)
        {
            Warp();
        }
    }

    Vector3 mouseVec, transPos, targetPos;
    void Warp()
    {
        audioSource.clip = clips[3];
        audioSource.Play();

        mouseVec = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mouseVec);
        targetPos = new Vector3(transPos.x, transPos.y, 0);
        transform.position = targetPos;
        warpSlider.value = 0;
    }

    public void God(bool isItem)
    {
        float showTime = 0f;
        float realTime = 0f;
        if (isItem)
        {
            showTime = 2.5f;
            realTime = 3f;
        }
        else
        {
            showTime = 1.5f;
            realTime = 1.5f;
        }

        if(godCoroutine != null)
            StopCoroutine(godCoroutine);
        godCoroutine = StartCoroutine(IsGod(showTime,realTime));
    }

    public void ShowFollower()
    {
        for (int i = 0; i < followers.Length; i++)
        {
            if(followers[i].activeSelf == false)
            {
                followers[i].SetActive(true);
                return;
            }

        }
    }

    public void SaveBoom()
    {
        if (boomIndex >= boomImages.Length)
            return;
        boomIndex++;
        boomImages[boomIndex - 1].gameObject.SetActive(true);
    }

    public void UseBoomCheck()
    {
        if(boomIndex >= 1 && Input.GetKeyDown(KeyCode.Space))
        {
            boomImages[boomIndex - 1].gameObject.SetActive(false);
            boomIndex--;
            UseBoom();
        }
    }

    void UseBoom()
    {
        CheatManager.Instance.AllEnemyDead(false);
    }

    IEnumerator IsGod(float showTime, float realTime)
    {
        
        isGod = true;
        anim.SetBool("isGod", isGod);
        yield return new WaitForSeconds(showTime);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(realTime - showTime);
        isGod = false;
        anim.SetBool("isGod", isGod);

    }
}
