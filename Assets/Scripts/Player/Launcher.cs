using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Launcher : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] float _bulletLifeTime = 2f;

    [Header("ObjectPoolSettings")]
    [SerializeField] int _defaultCapacity = 10;
    [SerializeField] int _maxSize = 20;   
    ObjectPool<Bullet> _pool;


    void Start()
    {
        ConstructPoolObject();
    }

    void Update()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Bullet bullet = _pool.Get();
            yield return new WaitForSeconds(_bulletLifeTime);
            if (bullet.gameObject.activeInHierarchy) _pool.Release(bullet);
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
        return bullet;
    }
    private void OnGetBullet(Bullet bullet)
    {
        // default values
        bullet.Init(delegate { _pool.Release(bullet); });
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
