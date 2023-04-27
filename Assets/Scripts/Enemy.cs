using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Enemy movement")]
    [SerializeField] float _movementSpeed = 1f;
    [Range(0f,7f)] [Tooltip("A larger value means more randomness in the direction of the initial movement. If 0, the object will move towards the center.")]
    [SerializeField] float _directionFactor = 7f;
    
    private Vector3 _direction;
    private Action<Enemy> _disappearAction;


    public void Init(Action<Enemy> disappearAction)
    {
        _disappearAction = disappearAction;

        _direction = new Vector3(-transform.position.x + Random.Range(-_directionFactor, _directionFactor), -transform.position.y + Random.Range(-_directionFactor, _directionFactor), 0).normalized;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * Time.deltaTime * _movementSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
            _disappearAction(this);
    }
}
