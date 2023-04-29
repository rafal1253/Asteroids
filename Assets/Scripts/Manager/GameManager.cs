using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager _levels;
    [SerializeField] EnemySpawner _enemySpawner;

    [SerializeField] int _startPlayerLifes = 3;

    int _currentLevelIndex;

    public int PlayerLifes 
    { 
        get { return _lifes; } 
        set 
        {
            _lifes = value; 
            EventManager.InvokeOnUpdatePlayerLifes(value);
            if (_lifes <= 0)
                GameOver();
        } 
    }
    private int _lifes;
    public int CollectedPoints { get { return _points; } set { _points = value; EventManager.InvokeOnUpdatePlayerPoints(value); } }
    private int _points;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.OnStartPlay += StartNewPlay;
    }
    private void OnDisable()
    {
        EventManager.OnStartPlay -= StartNewPlay;
    }

    private void SetLevel(int index)
    {

    }

    void StartNewPlay()
    {
        CollectedPoints = 0;
        PlayerLifes = _startPlayerLifes;

        _enemySpawner.StartSpawn();
    }

    void GameOver()
    {
        Data.EarnedPoints = CollectedPoints;

        Debug.LogWarning("ToChange");
        Data.ReachedLevel = 1;

        SceneLoader.Instance.LoadGameOverScreen();
    }


}
