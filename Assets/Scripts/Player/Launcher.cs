using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Launcher : MonoBehaviour
{
    [SerializeField] bool _autoFire = true;
    [SerializeField] float _shotsPerSec = 1f;
    float _nextFire = 0f;

    [Header("BULLET")]
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] float _bulletSpeed = 10f;

    [Header("OBJECT POOL SETTINGS")]
    [SerializeField] int _defaultCapacity = 10;
    [SerializeField] int _maxSize = 20;   
    ObjectPool<Bullet> _pool;


    void Start()
    {
        ConstructPoolObject();
    }

    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if ((_autoFire ? true : Input.GetButtonDown("Fire1")) && Time.time > _nextFire)
        {
            _nextFire = Time.time + 1 / _shotsPerSec;
            _pool.Get();
        }
    }


    #region ObjectPool
    private void ConstructPoolObject()
    {
        _pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, false, _defaultCapacity, _maxSize);
    }
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
        bullet.OnNewCreate(delegate { _pool.Release(bullet); });
        return bullet;
    }
    private void OnGetBullet(Bullet bullet)
    {
        // default values
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        
        bullet.gameObject.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        bullet.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, _bulletSpeed), ForceMode2D.Impulse);
    }
    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    #endregion
}
