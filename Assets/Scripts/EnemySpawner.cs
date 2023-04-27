using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] Enemy _enemyPrefab;

    [Header("ObjectPoolSettings")]
    [SerializeField] int _defaultCapacity = 10;
    [SerializeField] int _maxSize = 20;
    ObjectPool<Enemy> _pool;

    Camera _mainCam;
    Axis _spawnAxis;

    enum Axis { Horizontal, Vertical};

    // Start is called before the first frame update
    void Start()
    {
        ConstructPoolObject();
        _mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Enemy enemy = _pool.Get();
        }
    }


    #region ObjectPool

    private void ConstructPoolObject()
    {
        _pool = new ObjectPool<Enemy>(CreateEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy, true, _defaultCapacity, _maxSize);
    }
    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation, transform);
        return enemy;
    }
    private void OnGetEnemy(Enemy enemy)
    {
        float xSpawnPos;
        float ySpawnPos;
        _spawnAxis = Random.Range(0, 2) == 0 ? Axis.Horizontal : Axis.Vertical;

        if (_spawnAxis == Axis.Vertical)
        {
            xSpawnPos = Random.Range(0, 2) == 0 ? _mainCam.orthographicSize * _mainCam.aspect + 1 : -(_mainCam.orthographicSize * _mainCam.aspect + 1);
            ySpawnPos = Random.Range(_mainCam.orthographicSize + 1, -(_mainCam.orthographicSize + 1));
        }
        else
        {
            xSpawnPos = Random.Range(_mainCam.orthographicSize * _mainCam.aspect + 1, -(_mainCam.orthographicSize * _mainCam.aspect + 1));
            ySpawnPos = Random.Range(0, 2) == 0 ? _mainCam.orthographicSize + 1 : -(_mainCam.orthographicSize + 1);
        }

        enemy.transform.position = new Vector2(xSpawnPos, ySpawnPos);

        enemy.Init(delegate { _pool.Release(enemy); });
        enemy.gameObject.SetActive(true);

    }
    private void OnReleaseEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
    #endregion
}
