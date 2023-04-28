using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PlayerShip : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] float _acceleration = 1f;
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] int _damageOnContact = 1;

    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        // Main motion control
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rb.AddForce(movementVector * _acceleration * Time.deltaTime, ForceMode2D.Impulse);


        // Maximum speed limitation
        if (_rb.velocity.magnitude > _maxSpeed)
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxSpeed);
        }
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

    public void Damage(int damageTaken)
    {
        GameManager.Instance.PlayerLifes -= damageTaken;

        // ADD teleport to safe area
    }


}
