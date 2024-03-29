using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("ENEMY PROPERTIES")]
    [SerializeField] int _startLifes = 1;
    [SerializeField] int _destroyPoints;
    int _actualLifes;


    [Header("ENEMY MOVEMENT")]
    [SerializeField] float _minSpeed = 1f;
    [SerializeField] float _maxSpeed = 4f;
    [Range(0f,5f)] [Tooltip("A larger value means more randomness in the direction of the initial movement. If 0, the object will move towards the center.")]
    [SerializeField] float _directionFactor = 5f;
    private Vector3 _direction;
    
    private Action<Enemy> _disappearAction;


    public void OnNewCreate(Action<Enemy> disappearAction)
    {
        _disappearAction = disappearAction;
    }
    public virtual void OnNewSpawn()
    {
        _actualLifes = _startLifes;
        _direction = new Vector3(-transform.position.x + Random.Range(-_directionFactor, _directionFactor), -transform.position.y + Random.Range(-_directionFactor, _directionFactor), 0).normalized;
    }

    public void RevertDirection()
    {
        _direction *= -1;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += _direction * Time.deltaTime * Random.Range(_minSpeed, _maxSpeed);
    }

    public void Damage(int damageTaken)
    {
        _actualLifes -= damageTaken;

        if (_actualLifes <= 0)
        {
            GameManager.Instance.CollectedPoints += _destroyPoints;
            _disappearAction(this);
        }
    }
}
