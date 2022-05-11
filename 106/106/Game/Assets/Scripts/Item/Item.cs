using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Player player => Player.Instance;
    GameManager gameManager => GameManager.Instance;

    public float speed;

    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    public enum ItemType
    {
        Power,
        God,
        HP,
        Pain,
        Boom,
        Follow,
    }

    public ItemType myItem;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        switch (myItem)
        {
            case ItemType.Power:
                spriteRenderer.color = Color.green;
                break;
            case ItemType.God:
                spriteRenderer.color = Color.yellow;
                break;
            case ItemType.HP:
                spriteRenderer.color = Color.blue;
                break;
            case ItemType.Pain:
                spriteRenderer.color = Color.red;
                break;
            case ItemType.Boom:
                spriteRenderer.color = Color.magenta;
                break;
            case ItemType.Follow:
                spriteRenderer.color = Color.cyan;

                break;
            default:
                break;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    void Use()
    {
        GameManager.Instance.itemScore += 10;
        UIController.Instance.SetItemUse(myItem);
        switch (myItem)
        {
            case ItemType.Power:
                player.BulletLevel++;
                break;
            case ItemType.God:
                player.God(true);
                break;
            case ItemType.HP:
                gameManager.HP += 10;
                break;
            case ItemType.Pain:
                gameManager.Pain -= 10;
                break;
            case ItemType.Boom:
                player.SaveBoom();
                break;
            case ItemType.Follow:
                player.ShowFollower();
                break;
            default:
                break;
        }

        Dead();
    }

    public void Dead()
    {
        Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            Use();
        }
    }
}
