using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bullet : MonoBehaviour
{
    public Action<Bullet> OnBulletDisabled;
    private float bulletSpeed;
    private float bulletDamage;
    public float bulletLifeTime = 5;
    public float bulletBornTime;

    private void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        if (Time.time - bulletBornTime > bulletLifeTime)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        bulletBornTime = Time.time;
    }

    private void OnDisable()
    {
        OnBulletDisabled?.Invoke(this);
    }

    public void SetBullet(float bulletDamage,float bulletSpeed)
    {
        this.bulletDamage = bulletDamage;
        this.bulletSpeed = bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if damage object Send Event   
    }
}
