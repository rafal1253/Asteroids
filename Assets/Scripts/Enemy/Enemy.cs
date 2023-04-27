using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy properties")]
    [SerializeField] int _startLifes = 1;
    [SerializeField] int _destroyPoints;
    [SerializeField] int _actualLifes;


    [Header("Enemy movement")]
    [SerializeField] float _movementSpeed = 1f;
    [Range(0f,7f)] [Tooltip("A larger value means more randomness in the direction of the initial movement. If 0, the object will move towards the center.")]
    [SerializeField] float _directionFactor = 7f;
    


    private Vector3 _direction;
    private Action<Enemy> _disappearAction;


    public void Init(Action<Enemy> disappearAction)
    {
        _disappearAction = disappearAction;
        _actualLifes = _startLifes;
        _direction = new Vector3(-transform.position.x + Random.Range(-_directionFactor, _directionFactor), -transform.position.y + Random.Range(-_directionFactor, _directionFactor), 0).normalized;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * Time.deltaTime * _movementSpeed;
    }



    public void Damage(int damageTaken)
    {
        _actualLifes -= damageTaken;

        if (_actualLifes <= 0) 
            _disappearAction(this);
    }
}
