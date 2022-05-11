using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyG : Enemy
{


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            GameObject Bullet = Instantiate(base.bullet, transform.position, transform.rotation);
            Bullet.GetComponent<Bullet>().power = power;
            audioSource.clip = clips[0];
            audioSource.Play();
            yield return new WaitForSeconds(1f);
        }
    }
}
