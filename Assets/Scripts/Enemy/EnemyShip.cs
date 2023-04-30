using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy
{
    [Header("LAUNCHER")]
    [SerializeField] Launcher _launcher;

    PlayerShip _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerShip>();
    }
    private void OnEnable()
    {
        if (_player.IsRespawning())
            _launcher.SwitchOnOff(false);

        EventManager.OnPlayerDeath += OnPlayerDeath;
        EventManager.OnPlayerRespawn += OnPlayerRespawn;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= OnPlayerDeath;
        EventManager.OnPlayerRespawn -= OnPlayerRespawn;


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

    private void OnPlayerDeath()
    {
        _launcher.SwitchOnOff(false);
    }
    private void OnPlayerRespawn()
    {
        _launcher.SwitchOnOff(true);
    }
}
