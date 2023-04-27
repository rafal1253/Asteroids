using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    private Action<Bullet> _disappearAction;

    public void Init(Action<Bullet> disappearAction)
    {
        _disappearAction = disappearAction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(_damage);
            _disappearAction(this);
        }
    }
}


