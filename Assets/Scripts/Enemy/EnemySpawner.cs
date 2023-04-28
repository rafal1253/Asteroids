using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] Enemy[] _enemyPrefabList;

    [Header("ObjectPoolSettings")]
    [SerializeField] int _defaultCapacity = 10;
    [SerializeField] int _maxSize = 20;
    ObjectPool<Enemy>[] _objectPoolList;
    ObjectPool<Enemy> _pool;

    Camera _mainCam;
    Axis _spawnAxis;

    enum Axis { Horizontal, Vertical};

    private void Awake()
    {
        _objectPoolList = new ObjectPool<Enemy>[_enemyPrefabList.Length];
    }
    void Start()
    {
        ConstructObjectPoolList();
        _mainCam = Camera.main;
    }

    void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Enemy enemy = _objectPoolList[Random.Range(0, _objectPoolList.Length)].Get();
        }
    }


    #region ObjectPool

    private void ConstructObjectPoolList()
    {
        for (int i = 0; i < _enemyPrefabList.Length; i++)
        {
            ConstructPoolObject(i);
        }
    }
    private void ConstructPoolObject(int poolIndex)
    {
        _objectPoolList[poolIndex] = new ObjectPool<Enemy>(() => CreateEnemy(poolIndex), OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy, true, _defaultCapacity, _maxSize);
    }
    private Enemy CreateEnemy(int poolIndex)
    {
        Enemy enemy = Instantiate(_enemyPrefabList[poolIndex], transform.position, transform.rotation, transform);
        enemy.OnNewCreate(delegate
        {
            _objectPoolList[poolIndex].Release(enemy);
        });
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
        enemy.OnNewSpawn();
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
