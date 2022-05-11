using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyV : Enemy
{
    [SerializeField] float dashSpeed;

    void Start()
    {
        Attack();
        StartCoroutine(Dash());
    }

    void Attack()
    {
        for (int i = 0; i < 360; i+=30)
        {
           GameObject Bullet =  Instantiate(base.bullet, transform.position, transform.rotation);
           Bullet.GetComponent<Bullet>().power = power;
           Bullet.transform.rotation = Quaternion.Euler(0,0,i);
            audioSource.clip = clips[0];
            audioSource.Play();
        }
    }

    IEnumerator Dash()
    {
        float temp = speed;
        yield return new WaitForSeconds(1f);
        speed = dashSpeed;
        yield return new WaitForSeconds(0.5f); 
        speed = temp;

    }
}
