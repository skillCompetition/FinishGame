using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    AudioSource audionSource;

    private void Awake()
    {
        audionSource = GetComponent<AudioSource>();
        
    }

    void Start()
    {
        StartCoroutine(Show());
    }

    void Update()
    {
        FireCheck();
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);

    }

    float timer;
    void FireCheck()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            Fire();
            timer = 0;

        }
    }

    void Fire()
    {
        audionSource.Play();
        Instantiate(bullet, transform.position, transform.rotation);

    }
}
