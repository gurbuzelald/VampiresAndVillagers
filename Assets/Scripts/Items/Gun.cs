using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun :ItemEntity
{
    [SerializeField] private int remainingBullet;

    [SerializeField] int totalBulletInClip;

    public int maxBulletInClip;

    public float bulletDamageOfGun;

    public float bulletSpeedOfGun;

    public ParticleSystem muzzleParticle;

    public void ChangeClip()
    {
        int totalAmount = totalBulletInClip;

        int max = maxBulletInClip - totalAmount;

        int bulletCount = 0;

        if (remainingBullet >= max)
        {
            remainingBullet -= max;
            bulletCount = max;
        }
        else
        {
            bulletCount = remainingBullet;
            remainingBullet = 0;
        }

        if (bulletCount > 0)
        {
            totalBulletInClip += bulletCount;
        }
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

            StartCoroutine(GunAnimation());
            Bullet bullet = BulletPoll.Instance.GetBullet();

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
