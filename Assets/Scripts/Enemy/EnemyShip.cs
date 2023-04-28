using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy
{
    PlayerShip _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerShip>();
    }

    protected override void Update()
    {
        base.Update();
        Aim();
    }

    private void Aim()
    {
        // Main aiming control
        var dir = _player.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
