using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("BULLET PROPERTIES")]
    [SerializeField] int _damage = 1;
    [SerializeField] float _bulletLifeTime = 2f;

    private Action<Bullet> _disappearAction;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public void OnNewCreate(Action<Bullet> disappearAction)
    {
        _disappearAction = disappearAction;
    }

    void OnEnable()
    {
        StartCoroutine(DisappearAfterTime());
    }

    IEnumerator DisappearAfterTime()
    {
        yield return new WaitForSeconds(_bulletLifeTime);
        _disappearAction(this);
    }

    public void OnLaunching(float bulletSpeed)
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.AddRelativeForce(new Vector2(0, bulletSpeed), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(_damage);
            StopCoroutine(DisappearAfterTime());
            _disappearAction(this);
        }
    }
}


