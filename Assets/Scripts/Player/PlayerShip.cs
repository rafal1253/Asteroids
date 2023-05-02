using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class PlayerShip : MonoBehaviour, IDamageable
{
    [Header("PLAYER SHIP")]
    [SerializeField] SpriteRenderer _playerSprite;
    [SerializeField] Shield _shield;
    [SerializeField] Launcher _launcher;
    [Header("MOVEMENT")]
    [SerializeField] float _acceleration = 1f;
    [SerializeField] float _maxSpeed = 10f;
    [Header("DAMAGE")]
    [SerializeField] int _damageOnContact = 1;

    Rigidbody2D _rb;
    Collider2D _collider;
    bool _canMove = false;
    bool _respawning = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _launcher.SwitchOnOff(false);
    }

    private void OnEnable()
    {
        EventManager.OnStartPlay += OnStartPlay;
    }
    private void OnDisable()
    {
        EventManager.OnStartPlay -= OnStartPlay;
    }

    private void Update()
    {
        Aim();
    }
    void FixedUpdate()
    {
        Move();
    }

    private void Aim()
    {
        // Main aiming control
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Move()
    {
        if (_canMove)
        {
            // Main motion control
            Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _rb.AddForce(movementVector * _acceleration * Time.deltaTime, ForceMode2D.Impulse);


            // Maximum speed limitation
            if (_rb.velocity.magnitude > _maxSpeed)
            {
                _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxSpeed);
            }
        }
    }
    private void OnStartPlay()
    {
        _canMove = true;
        _launcher.SwitchOnOff(true);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Damage(_damageOnContact);
            Damage(_damageOnContact);
        }
    }
    public bool IsRespawning()
    {
        return _respawning;
    }
    public void Damage(int damageTaken)
    {
        GameManager.Instance.PlayerLifes -= damageTaken;

        EventManager.InvokeOnPlayerDeath();

        StartCoroutine(RespawnShip());
    }

    IEnumerator RespawnShip()
    {
        _respawning = true;
        // teleport to safe area nad respawn
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _playerSprite.enabled = false;
        _collider.enabled = false;
        _canMove = false;
        transform.position = Vector3.zero;

        _launcher.SwitchOnOff(false);

        yield return new WaitForSeconds(2f);
        _shield.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        _playerSprite.enabled = true;

        yield return new WaitForSeconds(2f);
        _collider.enabled = true;
        _shield.gameObject.SetActive(false);
        _canMove = true;
        _launcher.SwitchOnOff(true);

        _respawning = false;
        EventManager.InvokeOnPlayerRespawn();

    }
}
