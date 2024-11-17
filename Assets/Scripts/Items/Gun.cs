using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gun :ItemEntity
{
    public int remainingBullet;

   public int totalBulletInClip;

    public int maxBulletInClip;

    public float bulletDamageOfGun;

    public float bulletSpeedOfGun;

    public ParticleSystem muzzleParticle;

    public Transform bulletSpawnPoint;

    public Action<int, int> OnBulletAmountChanged;


    public void ChangeClip()
    {
        int bulletsNeeded = maxBulletInClip - totalBulletInClip;
        int bulletsToReload = Mathf.Min(bulletsNeeded, remainingBullet);
        totalBulletInClip += bulletsToReload;
        remainingBullet -= bulletsToReload;
        OnBulletAmountChanged?.Invoke(totalBulletInClip, remainingBullet);
    }

    bool isFire;

    IEnumerator GunAnimation()
    {
        isFire = true;
        muzzleParticle.Play();
        Vector3 originalPosition = transform.localPosition;
        transform.localPosition += new Vector3(0, 0, -0.1f);
        yield return new WaitForSeconds(0.05f);
        transform.localPosition = originalPosition;
        isFire = false;

    }

    private void Update()
    {
        if (!isGrabbed)
        {
            return;
        }

        if (isFire)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeClip();
        }
    }

    private void Fire()
    {
        if (totalBulletInClip > 0)
        {
            totalBulletInClip--;

            OnBulletAmountChanged?.Invoke(totalBulletInClip, remainingBullet);

            StartCoroutine(GunAnimation());

            Bullet bullet = BulletPoll.Instance.GetBullet();

            Vector3 targetPointBullet = grabedEntity.heroPlayerController.transform.forward * 1000;

            Vector3 bulletSpawnPointPosition = bulletSpawnPoint.position;
      
            bullet.transform.position = bulletSpawnPointPosition;

            Vector3 direction = (targetPointBullet - bulletSpawnPointPosition).normalized;

            bullet.transform.rotation = Quaternion.LookRotation(direction,Vector3.up); 

            bullet.SetBullet(bulletDamageOfGun,bulletSpeedOfGun);

            bullet.gameObject.SetActive(true);
        }
        else
        {
            if (remainingBullet > 0)
            {
                ChangeClip();
            }
        }
    }
}
