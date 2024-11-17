using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoll : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    private static BulletPoll _instance;

    private Queue<Bullet> bulletQueues = new Queue<Bullet>();
    public static BulletPoll Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BulletPoll>();

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public Bullet GetBullet()
    {
        if (bulletQueues.Count > 0)
        {
            return bulletQueues.Dequeue();
        }
        else
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.OnBulletDisabled += HandleBulletDisabled;
            return bullet;
        }
    }

    private void HandleBulletDisabled(Bullet bullet)
    {
        bulletQueues.Enqueue(bullet);
    }
}
