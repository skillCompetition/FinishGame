using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
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
            Vector3 vec = player.transform.position - transform.position;
            
            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

            GameObject Bullet =  Instantiate(base.bullet, transform.position, transform.rotation);
            Bullet.GetComponent<Bullet>().power = power;
            
            Bullet.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            
            audioSource.clip = clips[0];
            audioSource.Play();
            yield return new WaitForSeconds(1f);
        }

    }
}
