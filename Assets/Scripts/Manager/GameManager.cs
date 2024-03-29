using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("GAME PROPERTIES")]
    [SerializeField] LevelManager _levels;
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] float _nextLevelDelay = 2f;
    [SerializeField] int _startPlayerLifes = 3;
    int _currentLevelIndex;

    [Header("CURSOR")]
    [SerializeField] Texture2D _aimCursor;


    private int _lifes;
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
    private int _points;
    public int CollectedPoints
    { 
        get { return _points; } 
        set 
        { 
            _points = value;
            EventManager.InvokeOnUpdatePlayerPoints(value); 
        }
    }

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.OnStartPlay += StartNewPlay;
        EventManager.OnEndLevel += StartNextLevel;
    }
    private void OnDisable()
    {
        EventManager.OnStartPlay -= StartNewPlay;
        EventManager.OnEndLevel -= StartNextLevel;
    }


    void StartNewPlay()
    {
        // starting values
        CollectedPoints = 0;
        PlayerLifes = _startPlayerLifes;
        _currentLevelIndex = 0;

        Cursor.SetCursor(_aimCursor, new Vector2(_aimCursor.width/2, _aimCursor.height/2), CursorMode.Auto);

        EventManager.InvokeOnStartLevel(_currentLevelIndex);
        _enemySpawner.SpawnLevel(_levels.Levels[_currentLevelIndex]);
    }
    void StartNextLevel()
    { 
        StartCoroutine(StartNextLevelCoroutine()); 
    }
    IEnumerator StartNextLevelCoroutine()
    {
        yield return new WaitForSeconds(_nextLevelDelay);
        if (_levels.Levels.Length > _currentLevelIndex + 1)
        {
            _currentLevelIndex++;
            EventManager.InvokeOnStartLevel(_currentLevelIndex);
            _enemySpawner.SpawnLevel(_levels.Levels[_currentLevelIndex]);
        }
        else
            GameOver(); 
    }

    void GameOver()
    {
        Data.EarnedPoints = CollectedPoints;
        Data.ReachedLevel = _currentLevelIndex + 1;

        // back to default cursor
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        SceneLoader.Instance.LoadGameOverScreen();
    }


}
