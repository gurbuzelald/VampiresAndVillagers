using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bullet : MonoBehaviour
{
    public Action<Bullet> OnBulletDisabled;

    private void OnDisable()
    {
        OnBulletDisabled?.Invoke(this);
    }
}
