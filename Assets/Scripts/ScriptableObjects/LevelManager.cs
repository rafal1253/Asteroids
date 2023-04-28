using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelManager", menuName = "ScriptableObjects/LevelManager")]
public class LevelManager : ScriptableObject
{
    public Level[] Levels;

    [Serializable]
    public class Level
    {
        public int BigAsteroids;
        public int MediumAsteroids;
        public int SmallAsteroids;
        public int EnemyShips;
    }
}
