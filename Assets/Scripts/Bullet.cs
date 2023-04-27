using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Action<Bullet> _disappearAction;

    private void OnEnable()
    {

    }

    public void Init(Action<Bullet> disappearAction)
    {
        _disappearAction = disappearAction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
            _disappearAction(this);
    }
}


