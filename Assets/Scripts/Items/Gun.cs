using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun :ItemEntity
{
    [SerializeField] private int remainingBullet;

    [SerializeField] int totalBulletInClip;

    public int maxBulletInClip;

    private bool isGrabbed;

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

    private void Update()
    {
        if (!isGrabbed)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeClip();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            DeGrab();
        }
    }

    private void DeGrab()
    {

    }

    private void Fire()
    {
        if (totalBulletInClip > 0)
        {
            totalBulletInClip--;
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
