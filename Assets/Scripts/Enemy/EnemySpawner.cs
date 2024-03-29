using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("ENEMY")]
    [SerializeField] Enemy[] _enemyPrefabList;
    [Header("ENEMY SHIP SPAWNING")]
    [SerializeField] float _minTimeToSpawn = 7f;
    [SerializeField] float _maxTimeToSpawn = 20f;
    int _spawnedShips;
    bool _canSpawnShip;

    [Header("OBJECT POOL SETTINGS")]
    [SerializeField] int _defaultCapacity = 10;
    [SerializeField] int _maxSize = 20;
    ObjectPool<Enemy>[] _objectPoolList;

    Camera _mainCam;
    Axis _spawnAxis;
    enum Axis { Horizontal, Vertical};

    private void Awake()
    {
        _objectPoolList = new ObjectPool<Enemy>[_enemyPrefabList.Length];
        ConstructObjectPoolList();
    }
    void Start()
    {
        _mainCam = Camera.main;
    }

    public void SpawnLevel(LevelManager.Level level)
    {
        _spawnedShips = 0;
        _canSpawnShip = true;

        for (int i = 0; i < level.LevelAsteroidsAmounts().Length; i++)
        {
            for (int j = 0; j < level.LevelAsteroidsAmounts()[i]; j++)
            {
                _objectPoolList[i].Get();
            }
        }
        StartCoroutine(SpawnEnemyShips(level.EnemyShips));
    }

    IEnumerator SpawnEnemyShips(int ships)
    {
        while(ships > _spawnedShips)
        {
            yield return new WaitForSeconds(Random.Range(_minTimeToSpawn, _maxTimeToSpawn));
            if (_canSpawnShip)
            {
                _objectPoolList[3].Get();
                _spawnedShips++;
                _canSpawnShip = false;
            }
        }
    }

    private bool IsAllSpawnedEnemiesDestroyed()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
                return false;
        }
        return true;
    }

    private Vector2 NewPositionOutsideCamera()
    {
        float xSpawnPos;
        float ySpawnPos;
        Vector2 newPos;
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
        newPos = new Vector2(xSpawnPos, ySpawnPos);
        return newPos;
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
        enemy.transform.position = NewPositionOutsideCamera();
        enemy.OnNewSpawn();
        enemy.gameObject.SetActive(true);
    }
    private void OnReleaseEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        if (IsAllSpawnedEnemiesDestroyed())
            EventManager.InvokeOnEndLevel();
        if (enemy.GetType() == typeof(EnemyShip))
            _canSpawnShip = true;
    }
    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
    #endregion
}
